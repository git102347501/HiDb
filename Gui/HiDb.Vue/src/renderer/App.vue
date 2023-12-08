<template>
  <title-bar />
  <router-view v-slot="{ Component }">
    <component :is="Component" />
  </router-view>
</template>

<script setup lang="ts">
import TitleBar from "./components/common/TitleBar.vue";

//const { app } = require('electron');
//var { spawn  } = window.require('child_process');
const { spawn } = require('child_process');
function runAppInBackground(exePath) {
  const options = {
    detached: true,
    windowsHide: true
  };

  const child = spawn(exePath, [], options);

  child.unref(); // 让子进程独立运行，使其不受主进程关闭的影响
}
const path = require('path');
console.log(path);
const assetsPath = path.join(__dirname, '..', '..', '..', 'publish');
const exePath = path.join(assetsPath, 'HiDb.Api.exe');
// 调用 runAppInBackground 函数，传入你的 exe 文件路径
console.log(exePath);
runAppInBackground(exePath);
// let apiProcess = null;

// // 启动后台API
// function startApiProcess() {
//   apiProcess = spawn('path/to/api.exe', [], {
//     detached: true,
//     stdio: 'ignore'
//   });

//   apiProcess.unref();
// }

// function restartApiProcess() {
//   if (apiProcess) {
//     apiProcess.kill('SIGTERM');
//     apiProcess = null;
//   }

//   startApiProcess();
// }

// app.on('ready', () => {
//   startApiProcess();
// });

// app.on('before-quit', () => {
//   if (apiProcess) {
//     apiProcess.kill('SIGTERM');
//     apiProcess = null;
//   }
// });

</script>

<style>
</style>
