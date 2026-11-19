import re, html, sys, os, json, urllib.parse
# usage: parse.py <crawl_dir> <docs/api out dir> [summary.json]
root, out = sys.argv[1], sys.argv[2]
def txt(s):
    s = re.sub(r'<br\s*/?>', ' ', s); s = re.sub(r'<[^>]+>', '', s)
    return re.sub(r'\s+', ' ', html.unescape(s)).strip()
def safe(n): return re.sub(r'[^A-Za-z0-9_.-]', '_', html.unescape(n))[:200]
def short(t):  # shorten type names
    t = re.sub(r'Itslearning\.RestApi\.Entities\.|Itsolutions\.Itslearning\.Web\.RestApi\.', '', t)
    t = re.sub(r'EntityListOf', 'EntityList<', t) + ('>' if 'EntityListOf' in t else '')
    return t
def tables(s):
    res = []
    for tb in re.findall(r'<table class="help-page-table">(.*?)</table>', s, re.S):
        hdr = [txt(h) for h in re.findall(r'<th>(.*?)</th>', tb, re.S)]
        rows = []
        for tr in re.findall(r'<tr>(.*?)</tr>', tb.split('</thead>')[-1], re.S):
            rows.append([txt(c) for c in re.findall(r'<td[^>]*>(.*?)</td>', tr, re.S)])
        res.append((hdr, rows))
    return res
def shrink(o):
    if isinstance(o, list): return [shrink(o[0])] if o else []
    if isinstance(o, dict): return {k: shrink(v) for k, v in o.items()}
    return o
def json_sample(sec):
    m = re.search(r'<h4[^>]*>\s*application/json, text/json\s*</h4>\s*<div[^>]*>.*?<pre[^>]*>(.*?)</pre>', sec, re.S)
    if not m: return None
    raw = html.unescape(m.group(1)).strip()
    try: return json.dumps(shrink(json.loads(raw)), indent=2)
    except Exception: return raw
def md_table(hdr, rows):
    if not rows: return ''
    o = '| ' + ' | '.join(hdr) + ' |\n|' + '---|'*len(hdr) + '\n'
    for r in rows:
        r = [short(c).replace('|', '\\|') if i == 2 else c.replace('|', '\\|') for i, c in enumerate(r)]
        r = ['' if c == 'None.' else c for c in r]
        o += '| ' + ' | '.join(r) + ' |\n'
    return o
def trunc(s, n=60):
    l = s.splitlines(); return '\n'.join(l[:n] + (['... (truncated)'] if len(l) > n else []))

idx = open(f'{root}/index.html', encoding='utf-8').read()
# controller -> overview + links
ctrls = []
for m in re.finditer(r'<h3 id="([^"]+)">.*?</h3>(.*?)(?=<h3 id=|\Z)', idx, re.S):
    name, body = m.group(1), m.group(2)
    ov = re.search(r'<p>(.*?)</p>', body, re.S)
    links = re.findall(r'href="(/restapi/help/Api/[^"]+)"', body)
    if links: ctrls.append((name, txt(ov.group(1)) if ov else '', links))
os.makedirs(f'{out}/endpoints', exist_ok=True)
summary = []
for name, ov, links in ctrls:
    md = f'# {name}\n\n> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.\n\n'
    ov = re.sub(r'^Overview:\s*', '', ov)
    if ov: md += f'**Overview:** {ov}\n\n'
    md += '## Endpoints\n\n'
    for l in links: pass
    toc, body = '', ''
    for l in links:
        p = f'{root}/pages/{safe(urllib.parse.unquote(l.split("/Api/")[1]))}.html'
        if not os.path.exists(p): body += f'\n### (missing page) {l}\n'; continue
        s = open(p, encoding='utf-8').read()
        h1 = txt(re.search(r'<h1>(.*?)</h1>', s, re.S).group(1))
        method, path = h1.split(' ', 1)
        main = s.split('</h1>', 1)[1]
        desc = re.search(r'<div>\s*<p>(.*?)</p>', main, re.S)
        desc = txt(desc.group(1)) if desc else ''
        req, resp = (main.split('<h2>Response Information</h2>') + [''])[:2]
        uri_sec = req.split('<h3>Body Parameters</h3>')[0]
        body_sec = (req.split('<h3>Body Parameters</h3>') + [''])[1]
        anchor = re.sub(r'[^a-z0-9]+', '-', f'{method} {path.split("?")[0]}'.lower()).strip('-')
        deprecated = 'DEPRECATED' in desc.upper() or 'OBSOLETE' in desc.upper() or 'USE V2 INSTEAD' in desc.upper()
        toc += f'- `{method} {path.split("?")[0]}`{" ⚠️ deprecated" if deprecated else ""} — {desc[:110]}{"…" if len(desc) > 110 else ""}\n'
        summary.append((name, method, path.split('?')[0], desc, deprecated))
        b = f'\n### `{method} {path}`\n\n{desc or "_No description._"}\n\n'
        t = tables(uri_sec)
        if t: b += '**URI parameters**\n\n' + md_table(*t[0]) + '\n'
        bt = tables(body_sec.split('<h3>Request Formats</h3>')[0])
        bm = re.search(r'modelName=[^"]+">([^<]+)</a>', body_sec.split('<table')[0])
        if bm or bt:
            b += f'**Body** ({short(bm.group(1)) if bm else "see table"})\n\n'
            if bt: b += md_table(*bt[0]) + '\n'
        else:
            bp = txt(body_sec.split('<h3>Request Formats</h3>')[0])
            if bp and bp != 'None.': b += f'**Body:** {bp}\n\n'
        js = json_sample(body_sec)
        if js: b += '<details><summary>Request JSON sample</summary>\n\n```json\n' + trunc(js) + '\n```\n</details>\n\n'
        rd = re.search(r'<h3>Resource Description</h3>\s*<p>(.*?)</p>', resp, re.S)
        rm = re.search(r'</p>\s*(?:<a href="/restapi/help/ResourceModel\?modelName=[^"]+">([^<]+)</a>|([A-Za-z][^<\n]*?))\s*(?:<table|<h3)', resp, re.S)
        rtype = (rm.group(1) or rm.group(2)).strip() if rm else ''
        b += f'**Response:** `{short(rtype) or "?"}`' + (f' — {txt(rd.group(1))}' if rd and txt(rd.group(1)) else '') + '\n\n'
        rt = tables(resp.split('<h3>Response Formats</h3>')[0])
        if rt and rt[0][1]: b += md_table(*rt[0][0:2]) + '\n'
        js = json_sample(resp)
        if js: b += '<details><summary>Response JSON sample</summary>\n\n```json\n' + trunc(js) + '\n```\n</details>\n\n'
        body += b
    md += toc + '\n---\n' + body
    open(f'{out}/endpoints/{name}.md', 'w', encoding='utf-8').write(md)
# models
mdl = '# Resource models\n\n> Auto-generated from the help page ResourceModel pages. `Itslearning.RestApi.Entities.` prefix is stripped.\n> Some enum types from other namespaces (e.g. `Itsolutions.*`) return HTTP 500 on the help site and are listed as unresolved.\n\n'
names = sorted(os.listdir(f'{root}/models'))
for f in names:
    s = open(f'{root}/models/{f}', encoding='utf-8').read()
    n = f[:-5]
    if n.startswith('EntityListOf'): continue
    t = tables(s)
    mdl += f'\n## {short(n)}\n\n'
    desc = re.search(r'<h2>(.*?)</h2>\s*<p>(.*?)</p>', s, re.S)
    if desc and txt(desc.group(2)): mdl += txt(desc.group(2)) + '\n\n'
    mdl += (md_table(*t[0]) if t else '_No fields documented (possibly an enum or primitive)._\n')
refs = set()
for d in ('pages', 'models'):
    for f in os.listdir(f'{root}/{d}'):
        refs |= {safe(m) for m in re.findall(r'modelName=([^"]+)"', open(f'{root}/{d}/{f}', encoding='utf-8').read())}
missing = sorted(refs - {n[:-5] for n in names})
if missing:
    mdl += '\n## Unresolved models\n\nThese are referenced but their help pages returned an error (HTTP 500) when crawled. Infer values from JSON samples or live responses.\n\n' + ''.join(f'- `{m}`\n' for m in missing)
open(f'{out}/models.md', 'w', encoding='utf-8').write(mdl)
json.dump(summary, open(sys.argv[3], 'w'), indent=0) if len(sys.argv) > 3 else None
print(len(ctrls), 'controllers', len(summary), 'endpoints', len(names), 'models')
