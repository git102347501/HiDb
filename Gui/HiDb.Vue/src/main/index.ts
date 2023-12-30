'use strict'

import { BrowserWindow, app, session } from 'electron'
import InitWindow from './services/windowManager'
import DisableButton from './config/DisableButton'
const { spawn } = require('child_process');
const path = require('path');

// 后台子进程
let childProcess;

// 启动后台子进程
function runAppInBackground() {
  const assetsPath = path.join(__dirname, '..', 'publish');
  const exePath = path.join(assetsPath, 'HiDb.Api.exe');
  console.log(exePath);
  const options = {
    detached: true,
    windowsHide: true
  };

  childProcess = spawn(exePath, [], options);

  childProcess.unref(); // 让子进程独立运行，使其不受主进程关闭的影响
}
function runAppInMacBackground() {
  const assetsPath = path.join(__dirname, '..', 'publish');
  const dllPath = path.join(assetsPath, 'HiDb.Api.dll');

  childProcess = spawn('dotnet', [dllPath], {
    detached: true,
    stdio: 'ignore'
  });

  childProcess.unref();
}
function onAppReady() {
  console.log('process.env.NODE_ENV:' + process.env.NODE_ENV)
  if (process.platform === 'win32') {
    // 在Windows平台下的逻辑
    console.log('启动win后台子进程-HiDb.Api')
    runAppInBackground();
  } else if (process.platform === 'darwin') {
    console.log('启动mac后台子进程-HiDb.Api')
    runAppInMacBackground()
  } else {
    // 其他平台的逻辑
  }
  runAppInMacBackground()
  new InitWindow().initWindow()
  DisableButton.Disablef12()
  if (process.env.NODE_ENV === 'development') {
    const { VUEJS3_DEVTOOLS } = require("electron-devtools-vendor");
    session.defaultSession.loadExtension(VUEJS3_DEVTOOLS, {
      allowFileAccess: true,
    });
    console.log('已安装: vue-devtools')
  }
}

app.whenReady().then(onAppReady)
// 由于9.x版本问题，需要加入该配置关闭跨域问题
app.commandLine.appendSwitch('disable-features', 'OutOfBlinkCors')

app.on('window-all-closed', () => {
  // 所有平台均为所有窗口关闭就退出软件
  console.log('关闭后台子进程-HiDb.Api')
  if (childProcess) {
    // 向 HiDb.Api.exe 发送中止信号
    childProcess.kill();
  }
  app.quit()
})
app.on('browser-window-created', () => {
  console.log('window-created')
})

if (process.defaultApp) {
  if (process.argv.length >= 2) {
    app.removeAsDefaultProtocolClient('electron-vue-template')
    console.log('由于框架特殊性开发环境下无法使用')
  }
} else {
  app.setAsDefaultProtocolClient('electron-vue-template')
}
