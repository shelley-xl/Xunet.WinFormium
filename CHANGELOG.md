## [1.0.10](https://github.com/shelley-xl/Xunet.WinFormium/commit/5280b2a8d28251b84de2b0ff9f13abe646b110e3) (2024-08-14)

### Features

- 新增异常处理

## [1.0.9](https://github.com/shelley-xl/Xunet.WinFormium/commit/e25b41f52b9a2ee858eba201c4c2514e15830918) (2024-08-14)

### Features

- 新增WebApi支持
- 新增属性管理器

### Bug Fixes

- 修复已知问题

## [1.0.8](https://github.com/shelley-xl/Xunet.WinFormium/commit/c15c8c2638815403ce7907f9974f26e8b76e5bcc) (2024-08-12)

### Features

- 支持使用单例模式启动（互斥锁）
- 周期作业支持停止/恢复

## [1.0.7](https://github.com/shelley-xl/Xunet.WinFormium/commit/dd0b910a649eedc332f231f3731cd58aef5a11be) (2024-08-09)

### Features

- 定时作业支持Cron表达式
- 支持依赖注入
- 优雅的启动方式

### Bug Fixes

- 修复已知问题

## [1.0.6](https://github.com/shelley-xl/Xunet.WinFormium/commit/433ca04ff36f09c2db3e41b5f0fdce531e6ed297) (2024-08-07)

### Features

- 新增去首尾空格换行制表符

### Bug Fixes

- 周期作业，解决并发执行带来的问题
- 修复已知问题

## [1.0.5](https://github.com/shelley-xl/Xunet.WinFormium/commit/769621558215a8a90057842f7120aaad038a3d4c) (2024-08-07)

### Bug Fixes

- 修复已知问题

## [1.0.4](https://github.com/shelley-xl/Xunet.WinFormium/commit/6104c8b0cb8efff01ea6767955cdacd884b5fe20) (2024-08-07)

### Bug Fixes

- 优化

## [1.0.3](https://github.com/shelley-xl/Xunet.WinFormium/commit/fbf6c7f5b0f717185b7a3727f0660ad9bf8151c4) (2024-08-07)

### Features

- 新增HtmlAgilityPack辅助方法

### Bug Fixes

- 修复已知问题

## [1.0.2](https://github.com/shelley-xl/Xunet.WinFormium/commit/6162e076449172ff5d73cb697ecefe2d6d1b18e9) (2024-08-06)

### Bug Fixes

- 修复已知问题

## 1.0.1 (2024-08-05)

### Features

- 为爬虫而生
- 支持IHttpClientFacory，便于使用HttpClient发送网络请求
- 支持HTML页面解析，默认使用XPath
- 支持数据持久化，默认使用Sqlite数据库
- 支持周期作业，通过简单配置即可实现重复性作业
- 支持分布式雪花ID
- 无需设计界面，没有Designer.cs文件
- 无需新建Windows窗体，只需新建class类，继承BaseForm即可
- 支持重写属性和方法，便于扩展
- 内置界面日志输出，开箱即用
- 内置序列化和反序列化封装方法，便于调用
