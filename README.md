[![.NET](https://github.com/kou-hon/SqlCipherAndEFSample/actions/workflows/dotnet.yml/badge.svg)](https://github.com/kou-hon/SqlCipherAndEFSample/actions/workflows/dotnet.yml)

# SqliteCipherAndEFSample
SqliteCipherのサンプル

## 結論
sqlitepclraw.bundle_e_sqlcipherを参照してConnectionStringにPasswordを設定するだけでDB暗号化ができそう

## 結果
| pattern        | 結果  |
| -------------- | --- |
| パスワード正しい | アクセスＯＫ |
| パスワードなし   | アクセスＮＧ |
| パスワード誤り   | アクセスＮＧ |
