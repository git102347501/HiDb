<template>
    <div>
        <a-tabs v-model:activeKey="activeKey" >
            <a-tab-pane :key="1" tab="外观主题">
                <a-form
                ref="styleFormRef"
                name="styleForm-validation">
                    <a-form-item has-feedback label="样式主题" name="pass">
                        <a-select
                            ref="select"
                            v-model:value="styleConfig.theme" disabled
                            style="width: 150px">
                            <a-select-option value="light">明亮模式</a-select-option>
                            <a-select-option value="dark">暗黑模式</a-select-option>
                        </a-select>
                    </a-form-item>
                </a-form>
            </a-tab-pane>
            <a-tab-pane :key="2" tab="编辑器配置" force-render>
                <a-form
                    ref="editFormRef"
                    name="editForm-validation">
                    <a-form-item has-feedback label="数据表格双击" name="pass">
                        <a-select
                            ref="select"
                            v-model:value="editConfig.dclickMode"
                            style="width: 180px">
                            <a-select-option value="1">复制到粘贴板</a-select-option>
                            <a-select-option value="2">填充到编辑器</a-select-option>
                        </a-select>
                    </a-form-item>
                </a-form>
            </a-tab-pane>
            <a-tab-pane :key="3" tab="其他配置">
                暂无
            </a-tab-pane>
        </a-tabs>
    </div>
</template>
<script lang="ts" setup>
import { onMounted, UnwrapRef, reactive, ref } from 'vue';
import { dftConfig } from '../../utils/dft';

const activeKey = ref(1);
onMounted(() => {
    loadConfigData();
});
const styleConfig = reactive<any>({
    theme: 'light'
});
const editConfig = reactive<any>({
    dclickMode: '1'
});
const loadConfigData = ()=> {
    let data = localStorage.getItem('hidbconfig');
    console.log('loadConfigData');
    console.dir(data);
    if (data) {
      let curr = JSON.parse(data);
      if (curr.styleConfig) {
        styleConfig.theme = curr.styleConfig.theme;
      }
      if (curr.editConfig) {
        editConfig.dclickMode = curr.editConfig.dclickMode;
      }
    } else {
        styleConfig.theme = dftConfig.styleConfig.theme;
        editConfig.dclickMode = dftConfig.editConfig.dclickMode;
    }
}
defineExpose({
    styleConfig,
    editConfig
})
</script>