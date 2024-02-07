<template>
    <div class="tbedit">
        <div class="tool">
          <a-tooltip title="刷新表结构数据">
              <a-button @click="loadTableColumn" 
                :icon="h(RedoOutlined)"
                style="margin-right: 6px;">刷新</a-button>
          </a-tooltip>
          <a-tooltip title="添加表字段">
              <a-button @click="addNewColumnConfig" 
                :icon="h(RedoOutlined)"
                style="margin-right: 6px;">添加</a-button>
          </a-tooltip>
        </div>
        <div class="data">
          <a-table class="table"
              :columns="dbColumns" 
              @resizeColumn="handleResizeColumn"
              size="small"
              style="width: 100%;"
              :data-source="currdbData"
              :scroll="{ y: tableHeight, x: tableWidth }"
              :loading="loading"
              :pagination="false">
              <template #bodyCell="{ column, record }">
                <template v-if="allowEditColumns.includes(column.dataIndex)">
                  <div v-if="column.dataIndex == 'name'">
                    <a-input
                      v-model:value="record.name" placeholder="请输入字段名"
                      style="margin: -5px 0" />
                  </div>
                </template>
                <template v-if="column.dataIndex == 'type'">
                  <a-select
                    v-model:value="record.type"
                    show-search 
                    placeholder="请选择字段类型"
                    style="width: 140px"
                    :options="dbTypeOptions"
                    :filter-option="filterOption"
                  ></a-select>
                </template>
                <template v-if="column.dataIndex == 'isKeyValue'">
                  <a-switch v-model:checked="record.isKeyValue" />
                </template>
                <template v-if="column.dataIndex == 'isForeignKeyValue'">
                  <a-switch v-model:checked="record.isForeignKeyValue" />
                </template>
                <template v-if="column.dataIndex == 'allowNull'">
                  <a-switch v-model:checked="record.allowNull" />
                </template>
                <template v-if="column.dataIndex == 'dftValue'">
                  <a-input v-model:value="record.dftValue" placeholder="请输入默认值" />
                </template>
                <template v-if="column.dataIndex == 'numericPrecision'">
                  <a-input-number placeholder="精度" v-model:value="record.numericPrecision" :min="0" :max="999" :step="1" string-mode />
                </template>
                <template v-if="column.dataIndex == 'numSize'">
                  <a-input-number placeholder="小数位" v-model:value="record.numSize" :min="0" :max="999" :step="1" string-mode />
                </template>
                <template v-if="column.dataIndex == 'remark'">
                  <a-input type="text" v-model:value="record.remark" placeholder="字段备注" />
                </template>
                <template v-else-if="column.dataIndex === 'operation'">
                  <a-typography-link @click="saveColumnConfig(record)" :loading="record.loading" 
                    style="margin-right: 8px;">{{ record.isNew ? '保存新增' : '保存修改' }}</a-typography-link>
                  <a-typography-link  @click="deleteColumnConfig(record)" :loading="record.loading" 
                    style="margin-right: 8px;color:#ff7875">删除</a-typography-link>
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
import {  getTableColumnList, getDbType, updateTableColumn, addTableColumn, deleteTableColumn } from '../../api/table';
import {  RedoOutlined,DeleteOutlined } from '@ant-design/icons-vue';
import Modal from 'ant-design-vue/es/modal/Modal';
import { getGuid } from '@renderer/utils/guid';
import { isNull } from '@renderer/utils/common';
import { SelectProps } from 'ant-design-vue/es/vc-select/Select';

const props = defineProps(['database','mode','table','dbtype'])
const tableHeight = ref(document.body.clientHeight);
const sh = 160;

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
    sorter: false,
    resizable: true,
    width: 180
  },
  {
    title: '字段类型',
    dataIndex: 'type',
    sorter: false,
    resizable: true,
    width: 150
  },
  {
    title: '主键',
    dataIndex: 'isKeyValue',
    sorter: false,
    align: 'center',
    resizable: true,
    width: 70
  },
  {
    title: '外键',
    dataIndex: 'isForeignKeyValue',
    sorter: false,
    align: 'center',
    resizable: true,
    width: 70
  },
  {
    title: '允许null',
    dataIndex: 'allowNull',
    sorter: false,
    align: 'center',
    resizable: true,
    width: 100
  },
  {
    title: '默认值',
    dataIndex: 'dftValue',
    sorter: false,
    align: 'left',
    resizable: true,
    width: 130
  },
  {
    title: '精度',
    dataIndex: 'numericPrecision',
    sorter: false,
    align: 'center',
    resizable: true,
    width: 100
  },
  {
    title: '小数位',
    dataIndex: 'numSize',
    sorter: false,
    align: 'center',
    resizable: true,
    width: 100
  },
  {
    title: '备注',
    dataIndex: 'remark',
    sorter: false,
    align: 'left',
    resizable: true,
    width: 200
  },
  {
    title: '排序号',
    dataIndex: 'orderNo',
    sorter: false,
    align: 'center',
    resizable: true,
    width: 70
  },
  {
    title: '操作',
    dataIndex: 'operation',
    width: 120,
    fixed: 'right'
  }
]);
const tableWidth = ref(dbColumns.value.reduce((sum, column) => sum + column.width, 0));
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
    res.data.forEach(element => {
      element.isNew = false;
    });
    currdbData.value = res.data;
  },()=> {
    currdbData.value = [];
    loading.value = false;
  })
}
const dbTypeOptions = ref<SelectProps['options']>([]);
const loadDbType = () => {
  getDbType(props.dbtype).then(res => {
    dbTypeOptions.value = res.data.map(c => { return { value : c.name, label: c.name }});
  })
}

const filterOption = (input: string, option: any) => {
  return option.value.toLowerCase().indexOf(input.toLowerCase()) >= 0;
};
const saveColumnConfig = (data)=>{
  if (!data.name) {
    message.warning('字段名称不能为空');
    return;
  }
  if (!data.type) {
    message.warning('字段类型不能为空');
    return;
  }
  data.loading = true;
  if (!data.isNew) {
    updateTableColumn(props.dbtype, props.database, props.mode, props.table,
      data.name, data.type, !data.allowNull).then(res=>{
        if (res) {
          message.success('保存成功');
        } else {
          message.warning('保存失败');
        }
        data.loading = false;
    },()=>{ 
      message.warning('保存错误'); 
      data.loading = false; 
    })
  } else {
    addColumnConfig(data);
  }
}
const addNewColumnConfig = () =>{
  currdbData.value.push({
    key: getGuid(),
    isNew: true,
    loading: false,
    name: '',
    type: '',
    allowNull: true
  })
}
const addColumnConfig = (data)=>{
  data.loading = true;
  addTableColumn(props.dbtype, props.database, props.mode, props.table,
    data.name, data.type, !data.allowNull).then(res=>{
      if (res) {
        message.success('添加成功');
        data.isNew = false;
      } else {
        message.warning('添加失败');
      }
      data.loading = false;
  },()=>{ data.loading = false; })
}
const deleteColumnConfig = (data)=>{
  Modal.confirm({
      title: '确认要删除表字段[' + data.name + ']吗？',
      content: '请谨慎操作，删除不可恢复',
      okText: '确认',
      cancelText: '取消',
      onOk() {
        return new Promise((resolve, reject) => {
          deleteTableColumn(props.dbtype, props.database, props.mode, props.table,
            data.name).then(dres=>{
            if (dres) {
              message.success('删除成功');
              loadTableColumn();
            } else {
              message.warning('删除失败');
            }
            resolve(true);
          }, () => {
            message.error('删除错误');
            reject(false)
          })
        });
      },
    });
}
const clearData = ()=>{
  currdbData.value = [];
}

const handleResizeColumn = (w, col) => {
  col.width = w;
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
        height: calc(100% - 190px);

        .table {
          width: 100%;
          height: calc(100% - 190px);
        }
    }
}
</style>