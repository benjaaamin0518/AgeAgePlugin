chcp 65001
cd %1
kintone-customize-uploader --watch --base-url %2 --username %3 --password %4 dest/customize-manifest.json
exit %errorlevel% 
