# example-efcore-migrate

EntityFramework Core のマイグレーション実行時にアプリケーションコードとデータベースの状態がずれている場合の挙動を確認するサンプルプロジェクト

## 技術スタック

- Web アプリケーション プロジェクト
  - C# 12
  - .NET 8
  - ASP.NET Core
  - Entity Framework Core
- テストプロジェクト
  - NUnit
  - Testcontainers
- データストア
  - MySQL 8.0

## 実行方法

## 実行環境の前提
- .NET 8.0 SDK がインストールされている
- Docker が起動している

### Windows 環境でのアプリケーション起動

```powershell
.\tools\install_dotnet_ef.ps1
```

```powershell
docker compose up
.\tools\ef_migrations_add.ps1
.\tools\ef_database_update.ps1
```

データベースが起動しマイグレーションされた状態で、プロジェクト起動設定から起動する

## テスト実行

```powershell
dotnet test
```

## Linux 環境

VSCode + devcontainer を使ってサポートしようとしたが、Extension 周りが上手くいかず諦めた

dotnet コマンド自体は叩けるはず

## 実験結果

| アプリケーションモデル | データベーススキーマ | 結果概要 |
|----------------------|---------------------|----------|
| 追加カラム未存在 | 追加カラム存在 (NULL 非許容・デフォルト値なし) | 追加カラムに値が入らず、例外発生(INSERT 失敗) |
| 追加カラム未存在 | 追加カラム存在 (NULL 非許容・デフォルト値あり) | CRUD 可能 |
| 追加カラム未存在 | 追加カラム存在 (NULL 許容・デフォルト値なし) | CRUD 可能 |
| 追加カラム存在 | 追加カラム未存在 (NULL 非許容・デフォルト値なし) | 追加カラムが見つからず、例外発生(INSERT 失敗) |
| 追加カラム存在 | 追加カラム未存在 (NULL 非許容・デフォルト値あり) | 追加カラムが見つからず、例外発生(INSERT 失敗) |
| 追加カラム存在 | 追加カラム未存在 (NULL 許容・デフォルト値なし) | 追加カラムが見つからず、例外発生(INSERT 失敗) |

各テストケースの詳細は `ExampleEFCoreMigrate.Tests` プロジェクト内の該当ファイルを参照
