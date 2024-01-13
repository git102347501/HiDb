<template>
    <div class="content">
        <div class="title">
            <span class="label">HiDb 当前版本:</span>
            <span>{{version}}</span>
        </div>
        <div class="btn">
            <a-button @click="open" type="primary">检查更新</a-button>
        </div>
        <img class="logo" src="@renderer/assets/logo.png" alt="">
    </div>
</template>
<script setup lang="ts">
import packageJson from '../../../../package.json';
const version = packageJson.version;
const { shell } = require('electron');
const open = (url)=>{
    shell.openExternal('https://github.com/git102347501/HiDb/releases');
}
</script>

<style scoped lang="scss">

.content {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
    position: relative;
    padding: 10px 0 0 90px;

    .title {
        width: 100%;
        height: 45px;
        display: flex;
        flex-direction: row;
        align-items: center;
        
        .label {
            font-weight: 550;
            margin-right: 4px;
        }
    }
    .btn {
        width: 100%;
        height: 45px;
        display: flex;
        flex-direction: row;
    }
}
.logo {
    position: absolute;
    left: 4px;
    width: 80px;
    height: 80px;
}
</style>