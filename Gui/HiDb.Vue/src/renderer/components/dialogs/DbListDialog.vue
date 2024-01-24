<template>
    <div class="db-dialog">
        <a-table class="table"
            :columns="dbColumns" 
            size="small"
            :data-source="currdbData"
            :row-selection="rowSelection"
            :scroll="{ y: 500 }"
            :loading="dbloading"
            :pagination="false">
            <template #bodyCell="{ column, text, record }">
            <template v-if="['name', 'account', 'address'].includes(column.dataIndex)">
                <div>
                <a-input
                    v-if="editableData[record.key]"
                    v-model:value="editableData[record.key][column.dataIndex]"
                    style="margin: -5px 0"
                />
                <template v-else>
                    <span>
                    <span v-if="column.dataIndex == 'name'">
                        <sql-server v-if="record.type == 0"></sql-server>
                        <my-sql v-if="record.type == 1" :color="'dark'"></my-sql>
                        <pg-sql v-if="record.type == 2"></pg-sql>
                    </span>
                    {{ text }}
                    </span>
                </template>
                </div>
            </template>
            <template v-else-if="column.dataIndex === 'operation'">
                <div class="editable-row-operations">
                <span v-if="editableData[record.key]">
                    <a-typography-link @click="save(record.key)" style="margin-right: 8px;">保存</a-typography-link>
                    <a @click="cancel(record.key)" style="color: #a1a0a0">撤销</a>
                </span>
                <span v-else>
                    <a-typography-link @click="edit(record.key)"  style="margin-right: 8px;">
                    编辑
                    </a-typography-link>
                    <a-popconfirm title="确认删除吗?" @confirm="deleteDbRow(record.key)">
                    <a style="color: rgb(251, 78, 78);">删除</a>
                    </a-popconfirm>
                </span>
                </div>
            </template>
            </template>
        </a-table>
    </div>
</template>

<script lang="ts" setup>
import { onMounted, UnwrapRef, reactive, ref } from 'vue';
import { ConnectDbInput } from '../model/MainPageMode';
import { TableProps } from 'ant-design-vue';
import { DataType } from 'vue-request';
import { cloneDeep } from 'lodash-es';

const dbloading = ref(false);

// 表格数据列
const dbColumns = ref<any[]>([{
    title: '名称',
    dataIndex: 'name',
    sorter: false
    },{
    title: '地址',
    dataIndex: 'address',
    sorter: false
    },{
    title: '用户名',
    dataIndex: 'account',
    sorter: false,
    width: 140
    },{
    title: '操作',
    dataIndex: 'operation',
    width: 90,
    fixed: 'right'
}]);
const editableData: UnwrapRef<Record<string, ConnectDbInput>> = reactive({});
// 当前数据库表格数据
const currdbData = ref<ConnectDbInput[]>([]);
// 选择数据库事件
const rowSelection: TableProps['rowSelection'] = {
    onChange: (selectedRowKeys: string[], selectedRows: DataType[]) => {
        console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
        currSelectDb.value = selectedRows[0];
    },
    getCheckboxProps: (record: DataType) => ({
        disabled: record.name === 'Disabled User',
        name: record.name,
    }),
    type: 'radio',
    fixed: true
};

onMounted(() => {
    searchDbData();
});
// 获取本地数据库列表
const searchDbData = ()=> {
    dbloading.value = true;
    let data = localStorage.getItem('hidbdata');
    console.log('get-local:' + data);
    currdbData.value = data ? JSON.parse(data) : [];
    dbloading.value = false;
}

// 当前选择数据库
const currSelectDb = ref<DataType>();

const edit = (key: string) => {
    editableData[key] = cloneDeep(currdbData.value.filter(item => key === item.key)[0]);
};
const save = (key: string) => {
    Object.assign(currdbData.value.filter(item => key === item.key)[0], editableData[key]);
    delete editableData[key];
    console.log('save : ' + JSON.stringify(currdbData.value))
    localStorage.setItem('hidbdata', JSON.stringify(currdbData.value));
};
const cancel = (key: string) => {
    delete editableData[key];
};

const deleteDbRow = (key: string)=>{
    let index = currdbData.value.findIndex(item => key === item.key);
    currdbData.value.splice(index, 1);
    localStorage.setItem('hidbdata', JSON.stringify(currdbData.value));
}


defineExpose({
    currdbData,
    currSelectDb
})
</script>