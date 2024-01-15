<template>
    <div class="content">
        <div class="item" v-for="(item, index) in data" v-bind:key="index">
            <div class="icon">
                <img :src="item.icon" />
            </div>
            <div class="context">
                <div class="title">
                    {{ item.title }}
                </div>
                <div class="url">
                    <div v-if="item.title != 'Email'" @click="open(item.url)" class="externalLink">{{ item.desc }}</div>
                    <a class="externalLink" v-if="item.title == 'Email'" :href="item.href">{{ item.desc }}</a>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
const data: any[] = [
    {
        title: 'GitHub地址',
        desc: 'https://github.com/git102347501/HiDb',
        url:  'https://github.com/git102347501/HiDb',
        icon: '/assets/github.png'
    },
    {
        title: 'Windows Store',
        desc: 'https://apps.microsoft.com/detail/9N94LT5S8FD9',
        url: 'https://apps.microsoft.com/detail/9N94LT5S8FD9?hl=zh-cn&gl=cn',
        icon: '/assets/store.png'
    },
    {
        title: 'Author',
        desc: 'www.magicalconch.com',
        url:  'https://www.magicalconch.com',
        icon: '/assets/user.png'
    },
    {
        title: 'Email',
        desc: 'magicalconch@qq.com',
        icon: '/assets/email.png',
        href: 'mailto:magicalconch@qq.com?subject=来自HiDb的软件反馈&body=Hi magicalconch:'
    }
];
const { shell } = require('electron');
const open = (url)=>{
    shell.openExternal(url);
}
</script>

<style scoped lang="scss">

.content {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
    padding-top: 10px;

    .item {
        width: 100%;
        height: 80px;
        display: flex;
        flex-direction: row;

        .icon {
            height: 100%;
            width: 80px;
            display: flex;
            flex-direction: row;
            align-items: center;
            justify-content: center;
        }
        .context {
            flex: 1;
            height: 100%;
            .title {
                font-size: 18px;
                font-weight: 550;
                height: 40px;
                display: flex;
                flex-direction: row;
                align-items: center;
                justify-content: flex-start;
            }
            .url {
                .externalLink {
                    color: blue;
                    cursor: pointer;
                }
            }
        }
    }
}
</style>