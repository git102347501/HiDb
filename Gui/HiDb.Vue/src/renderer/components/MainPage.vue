<template>
    <a-layout class="main">
      <a-layout-header class="header">
        <div class="btn">
          <a-dropdown>
            <a-button ghost>操作</a-button>
            <template #overlay>
              <a-menu @click="selectedMenu">
                <a-menu-item key="1">数据库列表</a-menu-item>
                <a-menu-item key="2">新的连接</a-menu-item>
              </a-menu>
            </template>
          </a-dropdown>
        </div>
        <div class="title">
          <span class="info" @click="getLife">
            <span>
              <span v-if="lifeTest == 1">
                <wifi-outlined :width="20" :height="20" style="margin-right: 4px;" />服务连接中...</span>
              <span style="color: green" v-if="lifeTest == 9">
                <wifi-outlined :width="20" :height="20" style="margin-right: 4px;" />服务正常</span>
              <span style="color:red" v-if="lifeTest == -1">
                <wifi-outlined :width="20" :height="20" style="margin-right: 4px;" />服务连接失败</span>
              <span style="color: brown;" v-if="lifeTest == 0">
                <wifi-outlined :width="20" :height="20" style="margin-right: 4px;" />
                服务未连接
              </span>
          </span>
            <span style="margin-left: 10px;">|</span>
          </span>
          <span v-if="currDatabase.type != null && currDatabase.type!= undefined" class="info">
            <span style="color:#fff">{{ currDatabase.type === 1 ? 'MySQL' : 'SQL Server' }}</span>
            <span style="margin-left: 10px;">|</span>
          </span>
          <span v-if="currDatabase.name" class="info">
            <database-outlined :width="20" :height="20" />
            {{ currDatabase.name }}
          </span>
          <span v-if="currDatabase.address" class="info">
            <api-outlined :width="20" :height="20"  />
            {{ currDatabase.address }}
          </span>
          <span v-if="!currDatabase || !currDatabase.key" class="info">
            未连接到数据库
          </span>
          <a-dropdown v-if="currDatabase && currDatabase.key">
            <a-button ghost>
              <span v-if="currDatabase.account" class="info">
                <user-outlined :width="20" :height="20" />
                {{ currDatabase.account }}
              </span>
            </a-button>
            <template #overlay>
              <a-menu @click="selectedUserMenu">
                <a-menu-item key="1">注销</a-menu-item>
              </a-menu>
            </template>
          </a-dropdown>
        </div>
      </a-layout-header>
      <a-layout> 
          <a-layout-sider width="200" class="menu">
            <a-spin :spinning="currloading">
              <div class="search">
                <a-input-search v-model:value="searchValue" style="margin-bottom: 8px" placeholder="Search"  @search="onSearch" />
              </div>
              <div v-if="!currloading" class="tree">
                <a-tree style="height: 100%;"
                    v-model:expandedKeys="expandedMenuKeys"
                    v-model:selectedKeys="selectedMenuKeys"
                    :load-data="onLoadData" show-icon
                    @select="onSelect"
                    :tree-data="treeData"
                >
                  <template #switcherIcon="{ switcherCls }"><down-outlined :class="switcherCls" /></template>
                  <template #icon="{ type, selected }">
                    <template v-if="type === 'db'">
                      <database-outlined />
                    </template>
                    <template v-if="type === 'mode'">
                      <tablet-outlined />
                    </template>
                    <template v-if="type === 'table'">
                      <table-outlined />
                    </template>
                  </template>
                </a-tree>
                <a-empty v-if="!treeData.length" description="暂无数据库列表" />
              </div>
            </a-spin>
          </a-layout-sider>
          <a-layout class="work" style="padding: 0 8px 8px">
            <a-spin :spinning="currloading">
              <div class="tools">
                <a-tooltip title="执行">
                    <a-button @click="searchData" :icon="h(CaretRightOutlined)" style="margin-right: 6px;">执行</a-button>
                </a-tooltip>
                <a-tooltip title="撤销">
                    <a-button :icon="h(RedoOutlined)">撤销</a-button>
                </a-tooltip>
              </div>
              <a-layout-content  class="content"
                :style="{ background: '#fff', padding: '6px', margin: 0, minHeight: '280px' }" >
                <div class="sql">
                    <a-textarea class="input" ref="textarea" @pressEnter="searchData"
                        v-model:value="optValue" @click="handleClick"
                        placeholder="输入SQL语句后执行"
                        :auto-size="{ minRows: 4, maxRows: 15 }" />
                </div>
                <div class="data">
                    <a-table class="table"
                        v-if="isQuery"
                        :columns="columns" 
                        size="small"
                        :data-source="currData"
                        :scroll="{ y: pageHeight }"
                        :loading="loading"
                        :pagination="false"
                    >
                      <template #headerCell="{ column }"/>
                    </a-table>
                    <div class="msg" v-if="!isQuery">
                      影响行数: {{executeNum}}
                    </div>
                    <div  v-if="isQuery && pagination.total" class="table-line">
                      总记录行数:{{ pagination.total }} | 当前查询页大小:{{ pagination.pageSize }}
                    </div>
                </div>
              </a-layout-content>
            </a-spin>
          </a-layout>
      </a-layout>
      <a-modal v-model:open="openDbListDialog" title="数据库列表" width="680px" height="550px">
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
                <template v-if="['name', 'age', 'address'].includes(column.dataIndex)">
                  <div>
                    <a-input
                      v-if="editableData[record.key]"
                      v-model:value="editableData[record.key][column.dataIndex]"
                      style="margin: -5px 0"
                    />
                    <template v-else>
                      {{ text }}
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
                      <a-popconfirm title="确认删除吗?" @confirm="deleteDbRow(record.key)">
                        <a style="color: rgb(251, 78, 78);">删除</a>
                      </a-popconfirm>
                    </span>
                  </div>
                </template>
              </template>
          </a-table>
        </div>
        <template #footer>
          <a-button key="submit" type="default" @click="selectDb(true)">打开</a-button>
          <a-button key="submit" type="primary" @click="selectDbAndOpen">连接</a-button>
        </template>
      </a-modal>
      <a-modal v-model:open="openDbDialog" title="数据库连接">
        <div class="db-dialog">
          <a-form :model="openDbModel" :label-col="{ style: { width: '150px' } }" :wrapper-col="{ span: 14 }">
            <a-form-item label="数据库类型" name="type"
                :rules="[{ required: true, message: '请选择数据库类型!' }]">
              <a-select
                v-model:value="openDbModel.type"
                style="width: 100%"
                placeholder="请选择数据库类型"
                :options="dbTypeOptions"
              ></a-select>
            </a-form-item>
            <a-form-item label="数据库地址" name="address"
                :rules="[{ required: true, message: '请输入数据库地址' }]">
              <a-input v-model:value="openDbModel.address" placeholder="请输入数据库地址" />
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
        <template #footer>
          <a-button key="back" @click="cancelDbDialog">取消</a-button>
          <a-button key="submit" type="primary" :loading="submitOpenDbLoading" @click="submitOpenDb">连接</a-button>
        </template>
      </a-modal>
    </a-layout>
</template>

<script lang="ts" setup>
import { cloneDeep } from 'lodash-es';
import { h, ref, watch, onMounted, UnwrapRef, reactive  } from 'vue';
import { WifiOutlined,ApiOutlined,UserOutlined,AppstoreOutlined,DatabaseOutlined,FileAddOutlined,CaretRightOutlined,RedoOutlined, DownOutlined, TabletOutlined, TableOutlined, FrownOutlined, FrownFilled  } from '@ant-design/icons-vue';
import { getDb,getMode,getTable } from '../api/menu';
import { getSearch,execute} from '../api/search';
import { connectDb } from '../api/datasource';
import type { MenuTheme,TreeProps,TableProps, MenuProps  } from 'ant-design-vue';
import { message } from 'ant-design-vue';
import { ConnectDbInput } from './model/MainPageMode';
import { DataType } from 'vue-request';
import { getGuid } from '@renderer/utils/guid';
import { getuid } from 'process';
import { life } from '../api/life';

  const sh = 320;
  const pageHeight = ref(0);
  const loading = ref(false);
  const dbloading = ref(false);
  onMounted(() => {
    pageHeight.value = document.body.clientHeight - sh;
    console.log('onMounted:' + pageHeight.value);
    window.addEventListener('resize', onResize)
  });
  const onResize = () => {
    pageHeight.value = document.body.clientHeight - sh;
    console.log('onResize:' + pageHeight.value);
  };
  const optValue = ref<string>(''); // 执行SQL
  const searchValue = ref<string>(''); // 左侧搜索内容
  const expandedMenuKeys = ref<string[]>([]); // tree搜索key
  const selectedMenuKeys = ref<string[]>([]); // tree选择key
  const executeNum = ref(0); // 影响行数
  const isQuery = ref(true); // 是否走查询
  const dbTypeOptions = [{
    value: 0,
    label: 'SqlServer',
  },{
    value: 1,
    label: 'MySql',
  },{
    value: 2,
    label: 'PgSql',
  }];
  const submitOpenDbLoading = ref(false);
  const isMore = ref(false);
  // 当前数据库信息
  const currDatabase = ref<ConnectDbInput>({
    key: null,
    name: '',
    account: '',
    passWord: '',
    address: '',
    type: 0,
    port: 0,
    trustCert: true,
    trustedConnection: false,
    encrypt: true,
    saveLocal: true
  });
  // tree数据
  const treeData = ref<TreeProps['treeData']>([
  ]);
  const openDbDialog = ref<boolean>(false);
  const openDbListDialog = ref<boolean>(false);
  const selectedUserMenu: MenuProps['onClick'] = ({ key }) => {
    if (key == '1') {
      clearCurrDbData();
    } 
  }
  const selectedMenu: MenuProps['onClick'] = ({ key }) => {
    if (key == '1') {
      submitOpenDbList();
    } else {
      openDbDialog.value = true;
      openDbModel.key = getGuid();
      openDbModel.account = '';
      openDbModel.passWord =  '';
      openDbModel.address = '';
      openDbModel.type = 0;
      openDbModel.port = 1433;
      openDbModel.trustCert = true;
      openDbModel.trustedConnection = false;
      openDbModel.encrypt = true;
    }
  };
  const submitIsMore = () => {
    isMore.value =!isMore.value;
  };

  // 加载数据库列表
  const loadDataBase = ()=>{
    getDb(currDatabase.value.type).then(res => {
      console.log(res);
      treeData.value = res.data.map(c=> {
        return {
          title: c.name,
          key: c.name,
          isLeaf: false,
          type: 'db'
        }});
      console.log(treeData.value);
    }, err => {
      message.error(err.message);
    })
  };
  const editableData: UnwrapRef<Record<string, ConnectDbInput>> = reactive({});

  const edit = (key: string) => {
    editableData[key] = cloneDeep(currdbData.value.filter(item => key === item.key)[0]);
  };
  const save = (key: string) => {
    Object.assign(currdbData.value.filter(item => key === item.key)[0], editableData[key]);
    delete editableData[key];
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
  // 清空当前数据库数据
  const clearCurrDbData = () => {
    treeData.value = [];
    currData.value = [];
    expandedMenuKeys.value = [];
    selectedMenuKeys.value = [];
    searchValue.value = '';
    isQuery.value = true;
    optValue.value = '';
    pagination.value.total = 0;
    pagination.value.pageSize = 100;
    currDbName.value = '';
    currDatabase.value = {
      key: null,
      name: '',
      account: '',
      passWord: '',
      address: '',
      type: 0,
      port: 0,
      trustCert: true,
      trustedConnection: false,
      encrypt: true,
      saveLocal: true
    };
    columns.value = [];
  }
  // 打开数据库连接
  const submitOpenDb = ()=> {
    clearCurrDbData();
    currloading.value = true;
    submitOpenDbLoading.value = true;
    connectDb(openDbModel).then(res=>{
      currloading.value = false;
      if (!res.data || !res.data || !res.data.success) {
        message.error(res.data.message);
        submitOpenDbLoading.value = false;
      } else {
        message.success('连接成功');
        openDbDialog.value = false;
        submitOpenDbLoading.value = false;
        currDatabase.value = openDbModel;
        // 保存到本地
        saveDbByLocal(openDbModel);
        // 加载数据库列表
        loadDataBase();
      }
    }, err => { 
      currloading.value = false;
      submitOpenDbLoading.value = false;
      message.error(err && err.message ? err.message : '连接失败');
    })
  };
  const saveDbByLocal = (data) => {
    searchDbData();
    console.log('saveDbByLocal');
    // 寻找相同地址，账号和类型的本地记录
    let index = currdbData.value.findIndex(c=> c.key == data.key);
    if (data.saveLocal){
      if (index != -1) {
        // 更新本地
        currdbData.value[index].passWord = data.passWord;
        currdbData.value[index].port = data.port;
        currdbData.value[index].trustCert = data.trustCert;
        currdbData.value[index].trustedConnection = data.trustedConnection;
        currdbData.value[index].encrypt = data.encrypt;
      } else {
        // 新增本地
        currdbData.value.push(data);
      }
    } else {
      if (index == -1) {
        // 不保存本地也没有，跳过
        return;
      } else {
        // 不保存移除本地
        currdbData.value.splice(index, 1);
      }
    }
    localStorage.setItem('hidbdata', JSON.stringify(currdbData.value));
  }
  // 打开数据库列表
  const submitOpenDbList = ()=>{
    openDbListDialog.value = true;
    searchDbData();
  }
  const currloading = ref<boolean>(false);
  const selectDb = (openDialog)=> {
    if (!currSelectDb.value || !currSelectDb.value.key) {
      message.error('请选择一个数据库');
      return;
    }
    let data = currSelectDb.value;
    if (openDialog) {
      openDbListDialog.value = false;
      openDbDialog.value = true;
    } else {
      openDbDialog.value = false;
      openDbListDialog.value = false;
    }
    openDbModel.key = data.key;
    openDbModel.account = data.account;
    openDbModel.passWord = data.passWord;
    openDbModel.address = data.address;
    openDbModel.type = data.type;
    openDbModel.port = data.port;
    openDbModel.trustCert = data.trustCert;
    openDbModel.trustedConnection = data.trustedConnection;
    openDbModel.encrypt = data.encrypt;
  }
  const selectDbAndOpen = ()=>{
    selectDb(false);
    submitOpenDb();
  }
  const currSelectDb = ref<DataType>();
  const rowSelection: TableProps['rowSelection'] = {
    onChange: (selectedRowKeys: string[], selectedRows: DataType[]) => {
      console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
      currSelectDb.value = selectedRows[0];
    },
    getCheckboxProps: (record: DataType) => ({
      disabled: record.name === 'Disabled User', // Column configuration not to be checked
      name: record.name,
    }),
    type: 'radio',
    fixed: true
  };
  // 搜索
  const onSearch = (searchValue: string) => {
    loadDataBase();
  };
  const cancelDbDialog = ()=>{
    openDbDialog.value = false;
  }
  const cancelDbListDialog = ()=>{
    openDbListDialog.value = false;
  }

  // 主菜单
  const items = ref([
  {
    key: '1',
    icon: () => h(AppstoreOutlined),
    label: '打开',
    title: 'open',
    children: [
      {
        key: '11',
        icon: () => h(DatabaseOutlined),
        label: '数据库列表',
        title: 'db list',
      },
      {
        key: '12',
        icon: () => h(FileAddOutlined),
        label: '新的链接',
        title: 'new db',
      }
    ],
  }
  ]);

  // tree 点击加载
  const onLoadData: TreeProps['loadData'] = treeNode => {
      return new Promise<void>(resolve => {
        if (treeNode.dataRef.children) {
          resolve();
          return;
        }
        console.log(treeNode);
        if (treeNode.dataRef.type === 'db') {
          currDbName.value = treeNode.title;
          getMode(treeNode.title, currDatabase.value.type).then(res=>{
            treeNode.dataRef.children = res.data.map(c => {
              return {            
                title: c.name,
                key: c.name,
                isLeaf: false,
                type: 'mode',
                database: treeNode.title
              }
            });
            treeData.value = [...treeData.value];
            resolve();
          },() => {
            message.error('获取数据库信息失败');
            resolve();
          })
        } else if (treeNode.dataRef.type === 'mode') {
          getTable(treeNode.dataRef.database, treeNode.title, currDatabase.value.type).then(res=>{
            treeNode.dataRef.children = res.data.map(c => {
              return {            
                title: c.name,
                key: c.name,
                isLeaf: true,
                type: 'table',
              }
            });
            treeData.value = [...treeData.value];
            resolve();
          })
        }  else {
          resolve();
        }
      });
  };

  // 选中表
  const currDbName = ref('');
  const onSelect = (selectedKeys, e)=>{
    console.log('onSelect');
    console.log(e);
    if (e && e.node) {
      if (e.node.dataRef.type === 'table') {
        if (currDatabase.value.type == 0) {
          optValue.value = 'select * from ' + e.node.dataRef.title;
        } else if (currDatabase.value.type == 1) {
          optValue.value = 'select * from ' + e.node.dataRef.title;
        }
      }
    }
  }

  // 数据监听
  watch(searchValue, value => {
      const expanded = treeData.value
          .map((item: TreeProps['treeData'][number]) => {
            if (item.title.indexOf(value) > -1) {
                //return getParentKey(item.key, gData.value);
            }
            return null;
          }).filter((item, i, self) => item && self.indexOf(item) === i);
              //expandedMenuKeys.value = expanded;
              searchValue.value = value;
              //autoExpandParent.value = true;
  });

  const lifeTest = ref<number>(0);
  const getLife = ()=> {
    lifeTest.value = 1;
    life().then(res=>{
      lifeTest.value = res.data == true ? 9 : -1;
    }, () => {
      lifeTest.value = -1;
    })
  }
  getLife();

  // 获取父key
  const getParentKey = (
      key: string | number,
      tree: TreeProps['treeData'],
      ): string | number | undefined => {
      let parentKey;
      for (let i = 0; i < tree.length; i++) {
          const node = tree[i];
          if (node.children) {
          if (node.children.some(item => item.key === key)) {
              parentKey = node.key;
          } else if (getParentKey(key, node.children)) {
              parentKey = getParentKey(key, node.children);
          }
          }
      }
      return parentKey;
  };

  // 表格数据列
  const columns = ref<any[]>([]);
  // 表格数据列
  const dbColumns = ref<any[]>([{
    title: '名称',
    dataIndex: 'name',
    sorter: false,
    width: 150
  },{
    title: '地址',
    dataIndex: 'address',
    sorter: false,
    width: 130
  },{
    title: '用户名',
    dataIndex: 'account',
    sorter: false,
    width: 90
  },{
    title: '操作',
    dataIndex: 'operation',
    width: 120,
    fixed: 'right'
  }]);
  // 主题
  const theme = ref<MenuTheme>('dark');
  // 当前表格数据
  const currData = ref<any[]>([]);
  // 当前数据库表格数据
  const currdbData = ref<ConnectDbInput[]>([]);
  // 表格分页信息
  const pagination = ref({
    total: null,
    pageSize: 100
  });
  const textarea = ref(null);
  const handleClick = ()=>{
    textarea.value.focus();
  }
  const getSelectedText = () => {
    const start = textarea.selectionStart;
    const end = textarea.selectionEnd;
    
    if (start !== end) {
      textarea.setSelectionRange(start, end);
      const selectedText = textarea.value.substring(start, end);
      return selectedText;
    } else {
      return optValue.value;
    }
  }

  // 获取本地数据库列表
  const searchDbData = ()=> {
    dbloading.value = true;
    let data = localStorage.getItem('hidbdata');
    currdbData.value = data ? JSON.parse(data) : [];
    dbloading.value = false;
  }

  // 表格主查询
  const searchData = () => {
    let sql = getSelectedText();
    if (!sql) {
      message.error('执行语句不能为空!');
      return;
    }
    if ((sql as any).toLowerCase().indexOf('select') == -1) {
      isQuery.value = false;
      loading.value = true;
      execute({
        database: currDbName.value ? currDbName.value : '',
        sql: sql
      }, currDatabase.value.type).then(res => {
        loading.value = false;
        executeNum.value = res.data;
      }, err => {
        loading.value = false;
        message.error(err && err.message ? err.message : '执行失败');
      })
    } else {
      isQuery.value = true;
      loading.value = true;
      getSearch({
        database: currDbName.value ? currDbName.value : '',
        sql: sql,
        pageSize: pagination.value.pageSize
      }, currDatabase.value.type).then(res => {
        loading.value = false;
        console.log(res);
        if (res && res.data && res.data.list && res.data.list.length > 0) {
          let obj  = res.data.list[0];
          let len = Object.keys(obj).length;
          columns.value = Object.keys(obj).map(key => ({
            title: key,
            dataIndex: key,
            sorter: false,
            width: 30 + (getMaxLength(res.data.list, key) * 7)
          }));
          currData.value = res.data.list;
          pagination.value.total = res.data.count;
        } else {
          currData.value = [];
          pagination.value.total = 0;
        }
      }, err => {
        loading.value = false;
        message.error(err && err.message ? err.message : '查询失败');
      })
    }
  }
  const getMaxLength = (objCollection, name)=>{
    let maxLength = 0;
    let length = objCollection.length > 20 ? 20 : objCollection.length;
    for (var i = 0; i < length; i++) {
      // 获取当前对象的name属性长度
      var nameLength = objCollection[i][name] ? objCollection[i][name].length : 0;
      
      // 如果当前长度大于最大长度，则更新最大长度
      if (nameLength > maxLength) {
        maxLength = nameLength;
      }
    }
    if (maxLength < name.length) {
      return name.length;
    } else {   
      return maxLength;
    }
  }
</script>

<style lang="scss" scoped>
  
  .ant-row-rtl #components-layout-demo-top-side-2 .logo {
    float: right;
    margin: 16px 0 16px 24px;
  }
  
  .site-layout-background {
    background: #fff;
  }
  .main {
    margin-top: 30px;
    width: 100%;
    height: calc(100vh - 30px);
    overflow-y: hidden;
    overflow-x: hidden;

    .header {
        height: 50px;
        width: 100%;
        margin: 0;
        padding: 0;
        z-index: 999;
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-between;

        .btn {
          width: 150px;
          height: 100%;
          display: flex;
          flex-direction: row;
          align-items: center;
          justify-content: flex-start;
          padding: 0 6px;
        }
        .title {
          color: #ffffff;
          margin-right: 12px;

          .info {
            margin-right: 12px;
            cursor: pointer;
          }
        }
    }
    .menu {
        width: 300px !important;
        max-width: 300px !important;
        min-width: 300px !important;
        height: calc(100% - 16px);
        display: flex;
        flex-direction: column;
        padding: 8px;
        background-color: #fff;

        .search {
            height: 45px;
            width: 100%;
            display: flex;
            flex-direction: row;
            align-items: center;
            justify-content: center;
        }

        .tree {
            width: 100%;
            max-height: calc(100vh - 45px);
            overflow-y: auto;
        }
    }
    .work {
        height: calc(100vh - 80px);
        width: calc(100% - 350px);    

        .tools {
            width: 100%;
            height: 40px;
            padding: 0 12px;
            display: flex;
            flex-direction: row;
            align-items: center;
            justify-content: flex-start;
        }
        .content {
            width: 100%;
            height: 100%;
            display: flex;
            flex-direction: column;
            padding: 0;
            margin: 0;

            .sql {
                width: 100%;
                min-height: 105px;
                .input {
                    height: calc(100% - 8px);
                    margin: 4px;
                    width: calc(100% - 8px);
                }
            }
            .data {
                width: 100%;
                height: 100%;

                .table {
                  width: 100%;
                  height: calc(100% - 30px);
                }
                .table-line {
                  width: 100%;
                  height: 30px;
                  font-size: 12px;
                  color: #333333;
                  padding: 0 6px;
                  display: flex;
                  flex-direction: row;
                  align-items: center;
                  justify-content: flex-start;
                  background-color: #f5f5f5;
                }

                .msg {
                  width: 100%;
                  height: 100%;
                  display: flex;
                  flex-direction: column;
                  align-items: flex-start;
                  justify-content: flex-start;
                }
            }
        }
    }
  }
  .db-dialog {
    padding: 12px 0px 0px 0px;
    margin-top: 12px;

    .more {
      width: 100%;
      height: 35px;
      display: flex;
      flex-direction: row;
      align-items: center;
      justify-content: center;
      .btn {
        width: 100%;
      }
    }
  }
</style>
  