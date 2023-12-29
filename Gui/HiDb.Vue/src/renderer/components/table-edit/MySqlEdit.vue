<template>
    <div class="content">
        <a-table class="table"
                :columns="dbColumns" 
                size="small"
                :data-source="currdbData"
                :scroll="{ y: 500 }"
                :loading="dbloading"
                :pagination="false">
                
              <template #bodyCell="{ column, text, record }">
                <template v-if="allowEditColumns.includes(column.dataIndex)">
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
                          <my-sql v-if="record.type == 1"></my-sql>
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
                      <a @click="cancel(record.key)" style="color: #555555">撤销</a>
                    </span>
                    <span v-else>
                      <a-typography-link @click="edit(record.key)"  style="margin-right: 8px;">
                        编辑
                      </a-typography-link>
                    </span>
                  </div>
                </template>
              </template>
        </a-table>
    </div>
</template>
<script setup lang="ts">
import { cloneDeep } from 'lodash-es';
import { UnwrapRef, reactive, ref } from 'vue';
import { ConnectDbInput } from '../model/MainPageMode';

// 表格数据列
const dbColumns = ref<any[]>([{
    title: '名称',
    dataIndex: 'name',
    sorter: false,
    width: 150}
]);
// 可编辑的列
const allowEditColumns = ref<string[]>(['name','type']);
// 当前数据库表格数据
const currdbData = ref<any[]>([]);
// loading
const dbloading = ref(false);
// 编辑数据
const editableData: UnwrapRef<Record<string, ConnectDbInput>> = reactive({});

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

</script>
<style scoped lang="scss">
.content {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
    .table {
        width: 100%;
        height: 100%;
    }
}
</style>