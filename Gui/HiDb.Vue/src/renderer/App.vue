<template>
  <title-bar />
  <router-view v-slot="{ Component }">
    <component :is="Component" />
  </router-view>
</template>

<script setup lang="ts">
import TitleBar from "./components/common/TitleBar.vue";
// const { app } = require('electron'); 
// //const path = require('path');
// const { spawn } = require('child_process');

// let childProcess;
// function runAppInBackground(exePath) {
//   const options = {
//     detached: true,
//     windowsHide: true
//   };

//   childProcess = spawn(exePath, [], options);

//   childProcess.unref(); // 让子进程独立运行，使其不受主进程关闭的影响
// }

// const assetsPath = path.join(__dirname, '..', '..', '..', 'publish');
// const exePath = path.join(assetsPath, 'HiDb.Api.exe');
// // 调用 runAppInBackground 函数，传入你的 exe 文件路径
// runAppInBackground(exePath);
// let apiProcess = null;

// app.whenReady().then(() => {
//   const assetsPath = path.join(__dirname, '..', '..', '..', 'publish');
//   const exePath = path.join(assetsPath, 'HiDb.Api.exe');
//   runAppInBackground(exePath);
// });

// app.on('before-quit', () => {
//   if (childProcess) {
//     // 向 HiDb.Api.exe 发送中止信号
//     childProcess.kill();
//   }
// });
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
