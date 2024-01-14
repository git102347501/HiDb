Contribution Guide （贡献指南）

## Database support contribution （数据库支持贡献）
- HiDb DataProvider {database} You can contribute support code for relevant databases here.
- HiDb.DataProvider.{database} 你可以在这里贡献相关数据库的支持代码。

  To enable new type database support, please refer to HiDb The DataProvider layer abstracts and implements the relevant code, following the steps:
  - Create a new project hierarchy, such as HiDb DataProvider PgSql
  - Cite HiDb DataProvider project
  - Implement project abstract interfaces
  - Please refer to other hierarchical connection object pool management and implementation codes. After the implementation is completed, please create a new testing process code for the corresponding database in the Test layer to ensure that the contribution code can truly run.

  如需开启新的类型数据库支持，请引用HiDb.DataProvider层抽象并实现相关代码，步骤如下：
  - 创建新的项目分层，如：HiDb.DataProvider.PgSql
  - 引用HiDb.DataProvider项目
  - 实现项目抽象接口
  - 请注意参照其他分层的连接对象池管理和实现代码，在实现完成后，请在Test层新建对应数据库的测试流程代码，以确保贡献代码能够真正运行。

## Gui's contribution （Gui贡献）

- HiDb Vue Electron front-end project layer, where you can contribute any code related to the interface.
- HiDb.Vue Electron前端项目层，你可以在这里贡献关于界面的任何代码。

- Please pay attention to code specifications
  - Naming of files, variables, components, etc. according to project naming conventions
  - Ensure that your code is compatible with running on multiple platforms
  - Adhere to project hierarchy constraints and unify code responsibilities

- 请注意代码规范
  - 文件，变量，组件等命名等按照项目命名规范
  - 确保你的代码能够兼容多平台运行
  - 遵守项目分层约束代码职责统一

## 分支策略 Branch strategy
- Please fork the project and PR it to the project development layer for code review after completion of development
- 请fork项目，在完成开发后pr到项目develop层以待代码审阅
