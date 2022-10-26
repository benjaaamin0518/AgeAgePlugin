chcp 65001
cd %1
kintone-plugin-packer --watch %2 --ppk %3
exit %errorlevel% 
