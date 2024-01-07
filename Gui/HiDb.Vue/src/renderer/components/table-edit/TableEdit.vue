<template>
    <div class="tbedit">
        <div class="tool">
          <a-tooltip title="刷新表结构数据">
              <a-button @click="loadTableColumn" 
                :icon="h(RedoOutlined)"
                style="margin-right: 6px;">刷新</a-button>
          </a-tooltip>
        </div>
        <div class="data">
          <a-table class="table"
              :columns="dbColumns" 
              size="small"
              style="width: 100%;"
              :data-source="currdbData"
              :scroll="{ y: tableHeight }"
              :loading="loading"
              :pagination="false">
              <template #bodyCell="{ column, record }">
                <template v-if="allowEditColumns.includes(column.dataIndex)">
                  <div v-if="column.dataIndex == 'name'">
                    <a-input
                      v-model:value="record.name"
                      style="margin: -5px 0" />
                  </div>
                </template>
                <template v-if="column.dataIndex == 'type'">
                  <a-select
                    v-model:value="record.type"
                    show-search 
                    placeholder="选择字段类型"
                    style="width: 200px"
                    :options="dbTypeOptions"
                    :filter-option="filterOption"
                  ></a-select>
                </template>
                <template v-if="column.dataIndex == 'allowNull'">
                  <a-switch v-model:checked="record.allowNull" />
                </template>
                <template v-else-if="column.dataIndex === 'operation'">
                  <a-typography-link @click="saveColumnConfig(record)" :loading="record.loading" style="margin-right: 8px;">保存</a-typography-link>
                </template>
              </template>
          </a-table>
        </div>
    </div>
</template>
<script setup lang="ts">
import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { h, UnwrapRef, reactive, ref, watchEffect,onMounted,createVNode } from 'vue';
import { getTableColumnList, getDbType, updateTableColumn } from '../../api/table';
import {  RedoOutlined,DeleteOutlined } from '@ant-design/icons-vue';

const props = defineProps(['database','mode','table','dbtype'])
const tableHeight = ref(document.body.clientHeight);
const sh = 80;

onMounted(() => {
  tableHeight.value = document.body.clientHeight - sh;
  window.addEventListener('resize', onResize);
  loadDbType();
  loadTableColumn();
});
// 窗体大小改变事件
const onResize = () => {
  tableHeight.value = document.body.clientHeight - sh;
};
// 表格数据列
const dbColumns = ref<any[]>([{
    title: '名称',
    dataIndex: 'name',
    sorter: false
  },
  {
    title: '字段类型',
    dataIndex: 'type',
    sorter: false,
    width: 220
  },
  {
    title: '是否允许null',
    dataIndex: 'allowNull',
    sorter: false,
    width: 180
  },
  {
    title: '操作',
    dataIndex: 'operation',
    width: 120,
    fixed: 'right'
  }
]);
// 可编辑的列
const allowEditColumns = ref<string[]>(['name','allowNull']);
// 当前数据库表格数据
const currdbData = ref<any[]>([]);
// 编辑数据
const editableData: UnwrapRef<Record<string, any>> = reactive({});
// 编辑列
const edit = (key: string) => {
    editableData[key] = cloneDeep(currdbData.value.filter(item => key === item.key)[0]);
};
// 保存编辑
const save = (key: string) => {
    Object.assign(currdbData.value.filter(item => key === item.key)[0], editableData[key]);
    delete editableData[key];
    localStorage.setItem('hidbdata', JSON.stringify(currdbData.value));
};
// 撤销编辑
const cancel = (key: string) => {
    delete editableData[key];
};
// 加载
const loading = ref(false);
// 加载字段配置
const loadTableColumn = ()=>{
  loading.value = true;
  getTableColumnList({
    database: props.database,
    mode: props.mode,
    table: props.table
  }, props.dbtype).then((res: any) => {
    loading.value = false;
    currdbData.value = res.data;
  },()=> {
    currdbData.value = [];
    loading.value = false;
  })
}
const dbTypeOptions = ref([]);
const loadDbType = ()=>{
  getDbType(props.dbtype).then(res=>{
    dbTypeOptions.value = res.data.map(c => {return { value : c.name, label: c.name}});
  })
}

const filterOption = (input: string, option: any) => {
  return option.value.toLowerCase().indexOf(input.toLowerCase()) >= 0;
};
const saveColumnConfig = (data)=>{
  data.loading = true;
  updateTableColumn(props.dbtype, props.database, props.mode, props.table,
    data.name, data.type, !data.allowNull).then(res=>{
      if (res) {
        message.success('保存成功');
      } else {
        message.warning('保存失败');
      }
      data.loading = false;
  },()=>{ data.loading = false; })
}
const clearData = ()=>{
  currdbData.value = [];
}

watchEffect(()=>{
  console.log('watch');
  loadTableColumn();
});
</script>
<style scoped lang="scss">
.tbedit {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
    flex-wrap: nowrap;
    justify-content: flex-start;

    .tool {
      width: 100%;
      height: 40px;
      padding: 0 12px;
      display: flex;
      flex-direction: row;
      align-items: center;
      justify-content: flex-start;
    }

    .data {
        width: 100%;
        height: calc(100% - 40px);

        .table {
          width: 100%;
          height: 100%;
        }
    }
}
</style>