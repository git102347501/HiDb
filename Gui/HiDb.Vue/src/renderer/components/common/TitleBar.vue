<template>
  <div class="window-title" v-if="!IsUseSysTitle && isNotMac && !IsWeb">
    <!-- 软件logo预留位置 -->
    <div style="-webkit-app-region: drag" class="logo">
      <img src="@renderer/assets/icons/png/hidb.png" class="icon-logo" />
    </div>
    <!-- 菜单栏位置 -->
    <div></div>
    <!-- 中间标题位置 -->
    <div style="-webkit-app-region: drag" class="title">HiDb</div>
  </div>
  <div v-else-if="!IsUseSysTitle && !isNotMac" class="window-title"></div>
</template>

<script setup lang="ts">
import { ref } from "vue";
let { ipcRenderer, systemInfo } = window;

const IsUseSysTitle = ref(false);
const mix = ref(false);
const isNotMac = ref(false);
const __ISWEB__ = ref(false);
const IsWeb = ref(Boolean(__ISWEB__));

if (!ipcRenderer) {
  ipcRenderer = {} as any;
  ipcRenderer.on =
    ipcRenderer.invoke =
    ipcRenderer.removeAllListeners =
    (...args: any): any => {
      console.log("not electron");
    };
} else {
  isNotMac.value = systemInfo.platform !== "darwin";
  ipcRenderer.invoke("IsUseSysTitle").then((res) => {
    IsUseSysTitle.value = res;
  });
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.window-title {
  width: 100%;
  height: 0px;
  line-height:  0px;
  background-color: #ffffff;
  display: flex;
  -webkit-app-region: drag;
  position: fixed;
  top: 0;
  z-index: 99999;

  .icon-logo {
    width: 1em;
    height: 1em;
    vertical-align: -0.15em;
    fill: currentColor;
    overflow: hidden;
  }

  .title {
    text-align: center;
    color: #9d9d9d;
  }

  .logo {
    margin: 0 10px;
  }

  .controls-container {
    display: flex;
    flex-grow: 0;
    flex-shrink: 0;
    text-align: center;
    position: relative;
    z-index: 3000;
    -webkit-app-region: no-drag;
    height: 100%;
    width: 138px;
    margin-left: auto;

    .windows-icon-bg {
      display: inline-block;
      -webkit-app-region: no-drag;
      height: 100%;
      width: 33.34%;
      color: rgba(129, 129, 129, 0.6);

      .icon-size {
        width: 12px;
        height: 15px;
        vertical-align: -0.15em;
        fill: currentColor;
        overflow: hidden;
      }
    }

    .windows-icon-bg:hover {
      background-color: rgba(182, 182, 182, 0.2);
      color: #333;
    }

    .close-icon:hover {
      background-color: rgba(232, 17, 35, 0.9);
      color: #fff;
    }
  }
}
</style>
