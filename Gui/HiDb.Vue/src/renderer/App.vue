<template>
  <title-bar />
  <router-view v-slot="{ Component }">
    <component :is="Component" />
  </router-view>
</template>

<script setup lang="ts">
import TitleBar from "./components/common/TitleBar.vue";

//const { app } = require('electron');
// var { spawn  } = window.require('child_process');

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
