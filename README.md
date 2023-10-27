# MedalGame_RemoteControlServer

回路を自作したメダルゲームをスマートフォンからリモート操作をするためのサーバ側のコードです。
クライアント側は[こちら](https://github.com/UnknownSP/MedalGame_RemoteControlClient)

今のところはKAZAAANのみ対応。リポジトリは[こちら](https://github.com/UnknownSP/MedalGame_KAZAAAN)

## 動作例

スマートフォンからボールの発射ができます。

https://github.com/UnknownSP/MedalGame_RemoteControlClient/assets/39638661/c1e8fbc2-433c-40f4-b40c-48ec36e6c981

テストモード等も操作可能です。

https://github.com/UnknownSP/MedalGame_RemoteControlClient/assets/39638661/69507e4c-2c5b-4978-95ae-f9ba39885fdd

## 仕様

動画の送信はUnityの[RenderStreaming](https://docs.unity3d.com/ja/Packages/com.unity.renderstreaming@3.1/manual/index.html)を使用しています。

操作情報の送信や受取はTCPサーバとクライアントをUnity上でたてて行っています。

Assets/Sceneにメインのコード類が入っています。
