<template>
    <div class="db-dialog">
        <a-form :model="openDbModel" :label-col="{ style: { width: '140px' } }" :wrapper-col="{ span: 14 }">
        <a-form-item label="数据库类型" name="type"
            :rules="[{ required: true, message: '请选择数据库类型!' }]">
            <a-select
            @change="typeChange"
            v-model:value="openDbModel.type"
            style="width: 245px; margin-right: 6px;"
            placeholder="请选择数据库类型"
            :options="dbTypeOptions"
            ></a-select>
            <sql-server v-if="openDbModel.type == 0"></sql-server>
            <my-sql v-if="openDbModel.type == 1"></my-sql>
            <pg-sql v-if="openDbModel.type == 2"></pg-sql>
        </a-form-item>
        <a-form-item label="数据库地址" name="address"
            :rules="[{ required: true, message: '请输入数据库地址' }]">
            <a-input style="width: 190px;" v-model:value="openDbModel.address" placeholder="请输入数据库地址" />
            <a-input style="width: 80px; margin-left: 4px;" v-model:value="openDbModel.port" placeholder="端口" />
        </a-form-item>
        <a-form-item label="登录名" name="account"
            :rules="[{ required: true, message: '请输入登录名!' }]">
            <a-input v-model:value="openDbModel.account" placeholder="请输入登录名" />
        </a-form-item>
        <a-form-item label="密码" name="passWord"
            :rules="[{ required: true, message: '请输入密码!' }]">
            <a-input-password v-model:value="openDbModel.passWord" placeholder="请输入密码" />
        </a-form-item>
        <a-row>
            <a-col :span="12">
            <a-form-item v-if="isMore" label="启用加密连接" name="encrypt">
                <a-checkbox v-model:checked="openDbModel.encrypt"></a-checkbox>
            </a-form-item>
            </a-col>
            <a-col :span="12">
            <a-form-item v-if="isMore" label="信任服务器证书" name="trustCert">
                <a-checkbox v-model:checked="openDbModel.trustCert"></a-checkbox>
            </a-form-item>
            </a-col>
            <a-col :span="12">
            <a-form-item v-if="isMore" label="保存到本地列表" name="saveLocal">
                <a-checkbox v-model:checked="openDbModel.saveLocal"></a-checkbox>
            </a-form-item>
            </a-col>
            <!-- <a-col :span="12">
            <a-form-item v-if="isMore" label="使用Windows身份验证" name="trustedConnection">
                <a-checkbox v-model:checked="openDbModel.trustedConnection"></a-checkbox>
            </a-form-item>
            </a-col> -->
        </a-row>
        <div class="more">
            <a-button @click="submitIsMore" class="btn" type="primary" ghost>
            {{ isMore ? '收缩' : '更多' }}</a-button>
        </div>
        </a-form>
    </div>
</template>

<script setup lang="ts">

import { getGuid } from '@renderer/utils/guid';
import { reactive, ref } from 'vue';
import { ConnectDbInput } from '../model/MainPageMode';
import { dbTypeOptions } from '../../utils/database';

const isMore = ref(false);
const openDbModel = reactive<ConnectDbInput>({
    key: getGuid(),
    name: '',
    account: '',
    passWord: '',
    address: '',
    type: 0,
    port: 1433,
    trustCert: true,
    trustedConnection: false,
    encrypt: true,
    saveLocal: true
});

const submitIsMore = () => {
    isMore.value = !isMore.value;
};

// 数据库类型更改默认端口
const typeChange = (e)=>{
    openDbModel.port = e.port;
}

const init = ()=>{
    openDbModel.key = getGuid();
    openDbModel.account = '';
    openDbModel.passWord =  '';
    openDbModel.address = '';
    openDbModel.type = 0;
    openDbModel.port = dbTypeOptions[0].port;
    openDbModel.trustCert = true;
    openDbModel.trustedConnection = false;
    openDbModel.encrypt = true;
}

</script>