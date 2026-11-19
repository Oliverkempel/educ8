import re, html, sys, os, time, urllib.request, urllib.parse
base = 'https://sdu.itslearning.com'
root = sys.argv[1]
def get(url, path):
    if os.path.exists(path) and os.path.getsize(path) > 0: return open(path, encoding='utf-8').read()
    for i in range(3):
        try:
            req = urllib.request.Request(base+url, headers={'User-Agent':'Mozilla/5.0'})
            data = urllib.request.urlopen(req, timeout=30).read().decode('utf-8')
            open(path,'w',encoding='utf-8').write(data); time.sleep(0.15); return data
        except Exception as e:
            print('ERR', url, e, file=sys.stderr); time.sleep(2)
    return ''
idx = open(f'{root}/index.html', encoding='utf-8').read()
links = sorted(set(re.findall(r'href="(/restapi/help/Api/[^"]+)"', idx)))
models = set()
for i, l in enumerate(links):
    name = urllib.parse.unquote(l.split('/Api/')[1])
    safe = re.sub(r'[^A-Za-z0-9_.-]', '_', name)[:200]
    d = get(l, f'{root}/pages/{safe}.html')
    models |= set(re.findall(r'href="/restapi/help/ResourceModel\?modelName=([^"]+)"', d))
print('pages', len(links))
seen = set()
while models - seen:
    m = (models - seen).pop(); seen.add(m)
    safe = re.sub(r'[^A-Za-z0-9_.-]', '_', html.unescape(m))[:200]
    d = get('/restapi/help/ResourceModel?modelName=' + m, f'{root}/models/{safe}.html')
    models |= set(re.findall(r'href="/restapi/help/ResourceModel\?modelName=([^"]+)"', d))
print('models', len(seen))
