watch
---

A very simple `watch`-like clone for windows. No directory watching (yet). No frills.

why
---

I haven't found a `watch` in GOW, and the cmd / pwsh code to accomplish the same task
is horrid.

building
---
You'll need at least dotnet 6.0. If you like an easy life, install node:
```
npm ci
npm run publish
```

(artifact will appear in the `Publish` folder)

If you don't have (and really don't want to install) node, check out the commandline
for the "publish" script in package.json, or roll your own.

usage
---

Try `--help`.

Generally: `watch -n 1 some_command` runs `some_command` once a second (roughly) and prints
the output.

it's fat
---

Yes, this is a chonky boi when published (11mb), but it was quick to bang out in .net - if you
want to keep it smaller, you probably only need, from a regular build:
- watch.exe
- watch.dll
- PeanutButter.EasyArgs.dll
- PeanutButter.DuckTyping.dll
- PeanutButter.Utils.dll

for a total of about 721k, which is still chonky compared with the native `watch` coming
in at about 27k, but it's not like you have the option of using that on windows anyway :P
