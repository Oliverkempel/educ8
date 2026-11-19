# ApplicationInitialisationCacheWarmUp

> Auto-generated from an itslearning site's /restapi/help — do not hand-edit; regenerate instead.

**Overview:** Controller to help initialise the cache on start up. The cache is called from Web.config. It is triggered on application start-up /'iisreset' and application recycle. See tag 'applicationInitialization' in web.config.

## Endpoints

- `GET restapi/warmup/cache/extensionmodule/v1` — Load extension modules into memory cache on start-up.
- `GET restapi/warmup/cache/learningtool/v1` — Load learning tools into memory cache on start-up.
- `GET restapi/warmup/cache/plugin/v1` — Load plugins into memory cache on start-up.

---

### `GET restapi/warmup/cache/extensionmodule/v1`

Load extension modules into memory cache on start-up.

**Response:** `?`


### `GET restapi/warmup/cache/learningtool/v1`

Load learning tools into memory cache on start-up.

**Response:** `?`


### `GET restapi/warmup/cache/plugin/v1`

Load plugins into memory cache on start-up.

**Response:** `?`

