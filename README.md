# AgeAgePlugin

- kintoneのプラグイン/カスタマイズファイルの開発の際に必要な複数の開発環境情報を保存することが可能
- create-pluginを用いてプラグインのひな型から作成可能
- プラグイン/カスタマイズファイル用の定義ファイルをソフトから変更可能

<img src="https://user-images.githubusercontent.com/102705383/196067146-7082e1d6-79eb-4ad6-929b-5c8b72bf8f97.png" width="500px">
<img src="https://user-images.githubusercontent.com/102705383/196067173-26b1ff48-d139-4b41-8a8e-42847b47ea26.png" width="500px">
<img src="https://user-images.githubusercontent.com/102705383/196067188-8cca5144-2c52-45fe-a298-2d27254242c0.png" width="500px">

- プラグインのバージョンを実行ごとに任意のバージョン数更新する(自動バージョンアップ機能)
- タブ「プラグイン用」の実行ボタンを押下することにより、開発環境情報を用いてkintone-plugin-packer&kintone-plugin-uploader
  を並列に実行することが可能。
- タブ「カスタマイズファイル用」の実行ボタンを押下することにより、開発環境情報を用いてkintone-customize-uploaderを実行することが可能。
- 実行ログをソフト上で表示
- 実行ログを任意の場所に保存することができる
- 実行エラーログがMessageBoxにて確認することができる。
# インストール
```
npm install -g @kintone/create-plugin
npm install -g @kintone/plugin-packer
npm install -g @kintone/plugin-uploader
npm install -g @kintone/customize-uploader
```
# ドキュメント
[AgeAgePlugin利用方法](https://github.com/nishikawa-r/AgeAgePlugin/blob/main/AgeAgePlugin%E5%88%A9%E7%94%A8%E6%96%B9%E6%B3%95.pdf)
# リリース

・[v1.1.5(Pre-Release版)](https://github.com/nishikawa-r/AgeAgePlugin/releases)

# 画面

![image](https://user-images.githubusercontent.com/102705383/196067079-b3ef32af-61e3-484d-b950-144ee32b39f9.png)
![image](https://user-images.githubusercontent.com/102705383/196067057-fb856965-6e7d-44c0-a9eb-911e2e770f10.png)


# License

AgeAgePlugin is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).
