<template>
    <div class="main">
      <div class="header">
        <div class="btn">
          <a-dropdown>
            <a-button ghost>操作</a-button>
            <template #overlay>
              <a-menu value="1" @click="selectedMenu">
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
            <span style="color:#fff">{{ dbTypeOptions.find(c=> c.value == currDatabase.type).label }}</span>
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
      </div>
      <div class="content"> 
          <div class="menu" :style="{ 'width': menuWidth + 'px' }">
            <a-spin :spinning="currloading">
              <div class="search">
                <a-input-search v-model:value="searchValue" 
                  style="margin-bottom: 8px" placeholder="搜索数据库名称"  
                  @search="onSearch" />
              </div>
              <div v-if="!currloading" class="tree">
                <a-tree style="height: 100%;"
                    v-model:expandedKeys="expandedMenuKeys"
                    v-model:selectedKeys="selectedMenuKeys"
                    @rightClick="menuRightClick"
                    :load-data="onLoadData" show-icon
                    @select="onSelect"
                    :tree-data="treeData"
                >
                  <template #switcherIcon="{ switcherCls }">
                    <down-outlined :class="switcherCls" />
                  </template>
                  <template #icon="{ type, selected }">
                    <template v-if="type === 'db'">
                      <database-outlined />
                    </template>
                    <template v-if="type === 'mode'">
                      <tablet-outlined />
                    </template>
                    <template v-if="type === 'table-search'">
                      <a-input placeholder="Basic usage" />
                    </template>
                    <template v-if="type === 'table'">
                      <table-outlined />
                    </template>
                    <template v-if="type === 'tablenull'">
                      <borderless-table-outlined />
                    </template>
                  </template>
                  <template #title="{ key: treeKey, title }">
                    <a-dropdown :trigger="['contextmenu']">
                      <span>{{ title }}</span>
                      <template #overlay>
                        <a-menu @click="({ key: menuKey }) => onContextMenuClick(treeKey, menuKey,currRightData)">
                          <div v-if="currRightData.type == 'db'">
                            <a-menu-item key="11">查看数据库</a-menu-item>
                            <a-menu-item key="12">删除数据库</a-menu-item>
                          </div>
                          <div v-if="currRightData.type == 'mode'">
                            <a-menu-item key="21">刷新</a-menu-item>
                          </div>
                          <div v-if="currRightData.type == 'table'">
                            <a-menu-item key="31">查看表数据</a-menu-item>
                            <a-menu-item key="32">编辑表结构</a-menu-item>
                            <a-menu-item key="33">删除表</a-menu-item>
                          </div>
                        </a-menu>
                      </template>
                    </a-dropdown>
                  </template>
                </a-tree>
                <a-empty v-if="!treeData.length" description="暂无数据库列表" />
              </div>
            </a-spin>
          </div>
          <div :class="menuWidth <= 350 ? 'drap-line drap-line-left' : menuWidth >= 850 ? 'drap-line drap-line-right': 'drap-line'" @mousedown="leftResize"></div>
          <div class="work" :style="{ 'width': bodyWidth }">
            <div v-show="viewMode == 0">
              <div class="tools">
                <div>
                  <a-tooltip title="执行SQL区域内的SQL">
                      <a-button @click="searchData" 
                        :icon="h(CaretRightOutlined)" :disabled="!currDatabase || !currDatabase.key" 
                        style="margin-right: 6px;">执行</a-button>
                  </a-tooltip>
                  <a-tooltip title="清空SQL区域内容">
                      <a-button @click="clearData" :disabled="!currDatabase || !currDatabase.key" 
                      :icon="h(RedoOutlined)">清空</a-button>
                  </a-tooltip>
                </div>
                <div class="tool-right">
                  <DatabaseOutlined style="margin-right: 6px;" />
                  <a-select
                    v-model:value="currDbName"
                    :disabled="!currDatabase || !currDatabase.key" 
                    show-search
                    placeholder="选择当前数据库"
                    style="width: 200px"
                    :options="selectDbData"
                    :filter-option="selectDbfilterOption"
                  ></a-select>
                </div>
              </div>
              <div class="context" >
                <div class="sql" :style="{ 'height': editHeight + 'px' }">
                    <div ref="editorContainer" class="editor" style="height:100%; width: 100%;"></div>
                </div>
                <div :class="menuWidth <= 350 ? 
                  'drap-line drap-line-left' : menuWidth >= 850 ? 
                  'drap-line drap-line-right': 'drap-line'" 
                  @mousedown="editResize"></div>
                <div class="data"  :style="{ 'height': editBodyHeight }" 
                    v-if="!currloading">
                    <a-table class="table"
                        v-if="isQuery && !errorMsg"
                        :columns="columns" 
                        size="small"
                        :data-source="currData"
                        :scroll="{ y: pageHeight, x: '(100% - ' + (menuWidth + 30) + ')'}"
                        :loading="loading"
                        :pagination="false"
                    >
                      <template #headerCell="{ column }"/>
                    </a-table>
                    <div class="msg-line">
                      <div class="msg error" v-if="errorMsg">
                        执行错误: {{errorMsg}}
                      </div>
                      <div class="msg" v-if="isQuery && pagination.total != null &&  pagination.total != undefined">
                        总行数: {{ pagination.total }} ｜ 页行数: {{ pagination.pageSize }}
                      </div>
                      <div class="msg" v-if="!isQuery">
                        影响行数: {{executeNum}}
                      </div>
                      <div class="msg" v-if="elapsedTimeRef">
                        执行耗时：{{ elapsedTimeRef }} ms
                      </div>
                    </div>
                </div>
              </div>
            </div>
            <div v-if="viewMode == 3" class="work">
              <table-edit
                :database="editTableData.database" 
                :table="editTableData.table" 
                :mode="editTableData.mode" 
                :dbtype="editTableData.dbtype" />
            </div>
            <div  v-show="viewMode == 1" class="work" :style="{ 'width': bodyWidth }">
              编辑数据库
            </div>
          </div>
      </div>
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
      <a-modal v-model:open="openDbDialog" title="连接数据库">
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
        <template #footer>
          <a-button key="back" @click="cancelDbDialog">取消</a-button>
          <a-button key="submit" type="primary" :loading="submitOpenDbLoading" @click="submitOpenDb">连接</a-button>
        </template>
      </a-modal>
    </div>
</template>

<script lang="ts" setup>
import { cloneDeep } from 'lodash-es';
import { h, ref, watch, onMounted, UnwrapRef, reactive, createVNode, defineComponent  } from 'vue';
import MySql from './icons/MySql.vue';
import SqlServer from './icons/SqlServer.vue';
import PgSql from './icons/PgSql.vue';
import { message, Modal } from 'ant-design-vue';
import { ExclamationCircleOutlined, WifiOutlined,ApiOutlined,UserOutlined,BorderlessTableOutlined,DatabaseOutlined,FileAddOutlined,CaretRightOutlined,RedoOutlined, DownOutlined, TabletOutlined, TableOutlined, FrownOutlined, FrownFilled  } from '@ant-design/icons-vue';
import { getDb,getMode,getTable } from '../api/menu';
import { getSearch,execute} from '../api/search';
import { connectDb } from '../api/datasource';
import type { MenuTheme,TreeProps,TableProps, MenuProps  } from 'ant-design-vue';
import { ConnectDbInput } from './model/MainPageMode';
import { DataType } from 'vue-request';
import { getGuid } from '@renderer/utils/guid';
import { life } from '../api/life';
import * as monaco from 'monaco-editor';
import TableEdit from './table-edit/TableEdit.vue';
import { deleteTable } from '../api/table';
  const sh = 280;
  const pageHeight = ref(0);
  const dftPageHeight = ref(0);
  const loading = ref(false);
  const dbloading = ref(false);
  const editorContainer = ref<any>(null)
  let editor = null;
  onMounted(() => {
    pageHeight.value = document.body.clientHeight - sh;
    dftPageHeight.value = pageHeight.value;
    window.addEventListener('resize', onResize);
    initEdit();
  });
  const initEdit = (val = '')=>{
    console.log('initEdit')
    editor = monaco.editor.create(editorContainer.value, {
      value: val,
      language: "sql",
      folding: true, // 是否折叠
      foldingHighlight: true, // 折叠等高线
      foldingStrategy: "indentation", // 折叠方式  auto | indentation
      showFoldingControls: "always", // 是否一直显示折叠 always | mouseover
      disableLayerHinting: true, // 等宽优化
      emptySelectionClipboard: false, // 空选择剪切板
      selectionClipboard: false, // 选择剪切板
      automaticLayout: true, // 自动布局
      codeLens: false, // 代码镜头
      scrollBeyondLastLine: false, // 滚动完最后一行后再滚动一屏幕
      colorDecorators: true, // 颜色装饰器
      accessibilitySupport: "off", // 辅助功能支持  "auto" | "off" | "on"
      lineNumbers: "on", // 行号 取值： "on" | "off" | "relative" | "interval" | function
      lineNumbersMinChars: 3, // 行号最小字符   number
      readOnly: false, //是否只读  取值 true | false
      minimap: {
        enabled: false,
        
      }
    })
    monaco.languages.registerCompletionItemProvider('sql', {
      triggerCharacters: ['from', 'FROM'],
      provideCompletionItems: (model, position) => {
        let suggestions = []
        suggestions = tables.map((item: any) => {
          return {
            label: item,
            kind: item,
            insertText: item
          }
        })
        return {
          suggestions,
        }
      }
    })
  }
  const tables = ['table1','table2'];
  // 窗体大小改变事件
  const onResize = () => {
    pageHeight.value = document.body.clientHeight - sh;
    dftPageHeight.value = pageHeight.value;
    console.log('onResize:' + pageHeight.value);
  };
  const viewMode = ref(0); // 视图模式，0数据视图，1 编辑表视图 2编辑模式视图 3编辑数据库视图
  const editTableData = ref({
    table: '',
    database: '',
    mode: '',
    dbtype: 0
  }); // 当前标记表
  const searchValue = ref<string>(''); // 左侧搜索内容
  const expandedMenuKeys = ref<string[]>([]); // tree搜索key
  const selectedMenuKeys = ref<string[]>([]); // tree选择key
  const executeNum = ref(0); // 影响行数
  const isQuery = ref(true); // 是否走查询
  const errorMsg = ref(''); // 错误消息
  // // 菜单展开事件
  // watch(expandedMenuKeys, () => {
  //   console.log('expandedKeys', expandedMenuKeys);
  // });
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
  const menuWidth =  ref(350); // 菜单宽度
  const bodyWidth =  ref('calc(100% - 350px)'); // 内容宽度
  const mouseEventX =  ref<number>(0);
  
  const leftResize = (e: MouseEvent) => {
    // 处理拖动选中字问题
    document.onselectstart = function () {
      return false;
    }; 
    const changeWidth = (documentE: MouseEvent) => {
      let width = documentE.clientX < 350 ? 350 : documentE.clientX > 850 ? 850 : documentE.clientX;
      menuWidth.value = width;
      bodyWidth.value = 'calc(100% - ' + menuWidth.value + 'px)';
    };
    const mouseMove = (documentE: MouseEvent) => {
      mouseEventX.value = documentE.clientX;
      changeWidth(documentE);
    };
    const mouseUp = () => {
      document.removeEventListener('mousemove', mouseMove);
      document.removeEventListener('mouseup', mouseUp);
      // 拖拽完记得重新设置可以选中
      document.onselectstart = function () {
        return true;
      };
    };
    document.addEventListener('mousemove', mouseMove);
    document.addEventListener('mouseup', mouseUp);
  };
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

  const editHeight =  ref(105); // 菜单宽度
  const editBodyHeight =  ref('calc(100% - 105px)'); // 内容宽度
  const mouseEventY =  ref<number>(0);
  const editResize = (e: MouseEvent) => {
    // 处理拖动选中字问题
    document.onselectstart = function () {
      return false;
    }; 
    const changeWidth = (documentE: MouseEvent) => {
      let y = documentE.clientY - 100;
      let height = y < 105 ? 105 : y > 850 ? 850 : y;
      editHeight.value = height;
      let pgh = dftPageHeight.value - editHeight.value + 105;
      pageHeight.value = pgh > 100 ? pgh : 100;
      editBodyHeight.value = 'calc(100% - ' + menuWidth.value + 'px)';
    };
    const mouseMove = (documentE: MouseEvent) => {
      mouseEventY.value = documentE.clientY;
      changeWidth(documentE);
    };
    const mouseUp = () => {
      document.removeEventListener('mousemove', mouseMove);
      document.removeEventListener('mouseup', mouseUp);
      // 拖拽完记得重新设置可以选中
      document.onselectstart = function () {
        return true;
      };
    };
    document.addEventListener('mousemove', mouseMove);
    document.addEventListener('mouseup', mouseUp);
  }
  // 加载数据库列表
  const loadDataBase = ()=>{
    currloading.value = true;
    getDb(currDatabase.value.type, searchValue.value).then(res => {
      console.log(res);
      treeData.value = res.data.map(c=> {
        return {
          title: c.name,
          key: c.name,
          isLeaf: false,
          type: 'db'
        }});
      console.log(treeData.value);
      selectDbData.value = res.data.map(c=> {
        return {
          value: c.name,
          label: c.name
        }});
      currloading.value = false;
    }, err => {
      message.error(err.message);
      currloading.value = false;
    })
  };
  const selectDbData = ref<Array<string>>([]);
  const selectDbfilterOption = (input: string, option: any) => {
    return option.value.toLowerCase().indexOf(input.toLowerCase()) >= 0;
  };
  const editableData: UnwrapRef<Record<string, ConnectDbInput>> = reactive({});
  const currRightData = ref<any>(null);
  const menuRightClick = (e)=>{
    currRightData.value = e.node;
  }
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
  const childComponentRef = ref(null) // 创建一个ref对象
  // // 触发子组件方法
  // const triggerChildMethod = () => {
  //   childComponentRef.value.loadTableColumn();
  // }
  const onContextMenuClick = (treeKey: string, menuKey: string | number, data: any) => {
    if (menuKey == '11') {
      viewMode.value = 1;
    } else if (menuKey == '12') {
      viewMode.value = 1;
    } else if (menuKey == '21') {
      refDatabaseTable(currRightData.value.database, currRightData.value.title);
    } else if (menuKey == '31') {
      viewMode.value = 0;
      if (currDatabase.value.type == 0) {
        editor.setValue('select * from ' + currRightData.value.title);
      } else if (currDatabase.value.type == 1) {
        editor.setValue( 'select * from ' + currRightData.value.title);
      }
      searchData();
    } else if (menuKey == '32') {
      editTableData.value.database = currRightData.value.database;
      editTableData.value.table = currRightData.value.title;
      editTableData.value.mode = currRightData.value.mode;
      editTableData.value.dbtype = currDatabase.value.type;
      console.log(editTableData)
      viewMode.value = 3;
    } else if (menuKey == '33') {
      submitDeleteTable(currRightData.value.database, currRightData.value.title,
      currRightData.value.mode);
    } else if (menuKey == '34') {
      viewMode.value = 3;
    }
  };
  // 清空当前数据库数据
  const clearCurrDbData = () => {
    treeData.value = [];
    currData.value = [];
    expandedMenuKeys.value = [];
    selectedMenuKeys.value = [];
    searchValue.value = '';
    isQuery.value = true;
    editor.setValue('');
    pagination.value.total = undefined;
    pagination.value.pageSize = 100;
    currDbName.value = '';
    selectDbData.value = [];
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
    if (!data.name || data.name.length < 1) {
      // 默认名称为地址
      data.name = data.address;
    }
    if (data.saveLocal){
      if (index != -1) {
        // 更新本地
        currdbData.value[index].passWord = data.passWord;
        currdbData.value[index].name = data.name;
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
    console.log('save-local:' + JSON.stringify(currdbData.value));
    localStorage.setItem('hidbdata', JSON.stringify(currdbData.value));
  }
  // 打开数据库列表
  const submitOpenDbList = ()=>{
    openDbListDialog.value = true;
    searchDbData();
  }
  // 数据库类型更改默认端口
  const typeChange = (e)=>{
    if (e == 0) {
      openDbModel.port = 1433;
    } else if (e == 1) {
      openDbModel.port = 3306;
    } else if (e == 2){
      openDbModel.port = 5432;
    }
  }
  // 全局加载
  const currloading = ref<boolean>(false);
  // 选择db
  const selectDb = (openDialog)=> {
    if (!currSelectDb.value || !currSelectDb.value.key) {
      message.error('请选择一个数据库');
      return false;
    }
    let data = currdbData.value.filter(item => currSelectDb.value.key === item.key)[0];
    console.log('selectDb');
    console.log(data);
    openDbModel.key = data.key;
    openDbModel.account = data.account;
    openDbModel.passWord = data.passWord;
    openDbModel.address = data.address;
    openDbModel.type = data.type;
    openDbModel.port = data.port;
    openDbModel.trustCert = data.trustCert;
    openDbModel.trustedConnection = data.trustedConnection;
    openDbModel.encrypt = data.encrypt;
    console.log(openDbModel);
    if (openDialog) {
      openDbListDialog.value = false;
      openDbDialog.value = true;
    } else {
      openDbDialog.value = false;
      openDbListDialog.value = false;
    }
    return true;
  }
  // 选择数据库并打开连接
  const selectDbAndOpen = ()=>{
    if (selectDb(false)) {
      submitOpenDb();
    }
  }
  // 当前选择数据库
  const currSelectDb = ref<DataType>();
  // 选择数据库事件
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
  // 左侧菜单搜索事件
  const onSearch = () => {
    treeData.value = [];
    currData.value = [];
    expandedMenuKeys.value = [];
    selectedMenuKeys.value = [];
    loadDataBase();
  };
  // 关闭数据库连接
  const cancelDbDialog = ()=>{
    openDbDialog.value = false;
  }

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
          console.log(' currDbName.value:' +  currDbName.value);
          getMode(treeNode.title, currDatabase.value.type).then(res=>{
            treeNode.dataRef.children = res.data.map(c => {
              return {            
                title: c.name,
                key: treeNode.title + '_' + c.name,
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
            if (!res.data || !res.data || res.data.length < 1) {
              treeNode.dataRef.children = [{            
                title: '暂无表数据',
                key: '暂无表数据',
                isLeaf: true,
                disabled: true,
                type: 'tablenull',
              }];
            } else {
              treeNode.dataRef.children = res.data.map(c => {
                return {            
                  title: c.name,
                  key: c.name,
                  isLeaf: true,
                  type: 'table',
                  mode: treeNode.title,
                  database: treeNode.type == 'db' ? treeNode.title : treeNode.type == 'mode' ?  treeNode.parent.key : ''
                }
              });
            }
            treeData.value = [...treeData.value];
            resolve();
          })
        }  else {
          resolve();
        }
      });
  };

  // 刷新模式下表列表
  const refDatabaseTable = (database, mode, msg = true) => {
    let currDb = treeData.value.find(c => c.title == database);
    if (!currDb || !currDb.children) {
      message.error('未找到数据库节点信息');
      return;
    }
    let currMode = currDb.children.find(c => c.title == mode);
    if (!currMode || !currMode.children) {
      message.error('未找到模式节点信息');
      return;
    }
    getTable(database, mode, currDatabase.value.type).then(res=>{
      if (!res.data || !res.data || res.data.length < 1) {
        currMode.children = [{            
          title: '暂无表数据',
          key: '暂无表数据',
          isLeaf: true,
          disabled: true,
          type: 'tablenull',
        }];
      } else {
        currMode.children = res.data.map(c => {
          return {            
            title: c.name,
            key: c.name,
            isLeaf: true,
            type: 'table',
            mode: mode,
            database: database
          }
        });
      }
      treeData.value = [...treeData.value];
      if (msg) {
        message.success('刷新[' + database + '.' + mode + ']成功');
      }
    });
  }

  // 选中表
  const currDbName = ref('');
  // 树目录选择事件
  const onSelect = (selectedKeys, e)=>{
    console.log('onSelect');
    console.log(e);

    if (e && e.node) {
      if (e.node.dataRef.type === 'table') {
        if (currDatabase.value.type == 0) {
          editor.setValue('select * from ' + e.node.dataRef.title);
        } else if (currDatabase.value.type == 1) {
          editor.setValue( 'select * from ' + e.node.dataRef.title);
        }
        currDbName.value = e.node.dataRef.database;
        console.log('currDbName' + currDbName.value);
      }
    }
  }
  // 后台存活监听
  const lifeTest = ref<number>(0);
  // 监听连接
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
  const getSelectedText = () => {
    console.log('getSelectedText');
    const selection = editor.getSelection(); // 获取光标选中的值 
    const { startLineNumber, endLineNumber, startColumn, endColumn } = selection; 
    const value = editor.getModel().getValueInRange({
      startLineNumber: startLineNumber, 
      startColumn: startColumn, 
      endLineNumber: endLineNumber, 
      endColumn: endColumn
    });
    console.log('getValueInRange-res' + value);
    // 如果有选中文本
    if (!value) {
      return editor.getValue();
    } else {
      return value;
    }
  }

  // 获取本地数据库列表
  const searchDbData = ()=> {
    dbloading.value = true;
    let data = localStorage.getItem('hidbdata');
    console.log('get-local:' + data);
    currdbData.value = data ? JSON.parse(data) : [];
    dbloading.value = false;
  }
  const clearData = ()=> {
    editor.setValue('');
  }

  // 执行耗时/毫秒
  const elapsedTimeRef = ref<number>(0);
  const isSelect = (val)=>{
    if(val.includes('select') ){
      return true;
    }
    if(val.includes('show') ){
      return true;
    }
    return false;
  }
  // 表格主查询
  const searchData = () => {
    let sql = getSelectedText();
    if (!sql) {
      message.error('执行语句不能为空!');
      return;
    }
    if (!isSelect((sql as any).toLowerCase())) {
      isQuery.value = false;
      loading.value = true;
      execute({
        database: currDbName.value ? currDbName.value : '',
        sql: sql
      }, currDatabase.value.type).then(res => {
        loading.value = false;
        executeNum.value = res.data.changeCount;
        elapsedTimeRef.value = res.data.elapsedTime;
        errorMsg.value = res.data.message;
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
        elapsedTimeRef.value = res.data.elapsedTime;
        errorMsg.value = res.data.message;
        console.log(res);
        if (res && res.data && res.data.list && res.data.list.length > 0) {
          let obj  = res.data.list[0];
          columns.value = Object.keys(obj).map(key => ({
            title: key,
            dataIndex: key,
            sorter: false,
            width: 30 + (getMaxLength(res.data.list, key) * 10)
          }));
          currData.value = res.data.list;
          pagination.value.total = res.data.count;
        } else {
          columns.value = [];
          currData.value = [];
          pagination.value.total = 0;
        }
      }, err => {
        loading.value = false;
        message.error(err && err.message ? err.message : '查询失败');
      })
    }
  }
  const submitDeleteTable = (database, table, mode)=>{
    Modal.confirm({
      title: '确认要删除表[' + table + ']吗？',
      icon: createVNode(ExclamationCircleOutlined),
      content: '请谨慎操作，删除不可恢复',
      okText: '确认',
      cancelText: '取消',
      onOk() {
        return new Promise((resolve, reject) => {
          deleteTable(currDatabase.value.type, database, mode, table).then(dres=>{
            if (dres) {
              message.success('删除成功');
              refDatabaseTable(database, mode, false);
            } else {
              message.warning('删除失败');
            }
            resolve(true);
          }, () => {
            message.error('删除错误');
            reject(false)
          })
        }).catch(() => console.log('Oops errors!'));
      },
    });
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
    //margin-top: 30px;
    width: 100%;
    height: 100%;
    overflow-y: hidden;
    overflow-x: hidden;
    display: flex;
    flex-direction: column;

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
        background-color: rgb(24, 24, 40);

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

    .content{
      height: calc(100% - 50px);
      width: 100%;
      display: flex;
      flex-direction: row;

      
      .menu {
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
      .drap-line {
        height: 100%;
        width: 5px;
        background-color: #fffefe;
        cursor: col-resize;
        z-index: 999;
      }
      .drap-line-left {
        border-left: #dbd7d7 2px solid;
      }
      .drap-line-right {
        border-right: #dbd7d7 2px solid;
      }
      .work {
          width: 100%;
          height: calc(100vh - 50px);
          padding: 0;
          flex: 1;
          display: flex;
          flex-direction: column;

          .tools {
              width: 100%;
              height: 40px;
              padding: 0 12px;
              display: flex;
              flex-direction: row;
              align-items: center;
              justify-content: space-between;

              .tool-right {
              }
          }
          .context {
              width: 100%;
              height: calc(100% - 40px);
              display: flex;
              flex-direction: column;
              align-items: center;
              padding: 0;
              margin: 0;
              background: '#fff'; 
              padding: '6px'; 
              min-height: '280px';

              .drap-line {
                width: calc(100% - 12px);
                height: 1px;
                background-color: #fffefe;
                cursor: row-resize;
                z-index: 999;
                margin: 4px 0;
              }
              .drap-line-left {
                border-top: #dbd7d7 2px solid;
              }
              .drap-line-right {
                border-bottom: #dbd7d7 2px solid;
              }

              .sql {
                  width: calc(100% - 12px);
                  height: auto;
                  border: #cccccc 1px solid;
                  border-radius: 4px;
                  padding: 6px;

                  .input {
                      height: calc(100% - 8px);
                      margin: 4px;
                      width: calc(100% - 8px);
                  }
                  .editor {
                      height: 105px;
                      margin: 4px;
                      width: calc(100% - 8px);
                  }
              }
              .data {
                  width: 100%;

                  .table {
                    width: 100%;
                    padding: 0;
                    margin: 0;
                  }
                  .msg-line {
                    display: flex;
                    flex-direction: row;
                    align-items: center;
                    justify-content: flex-start;
                    width: 100%;
                    height: 30px;
                    font-size: 12px;
                    color: #333333;
                    padding: 0 6px;
                  }

                  .msg {
                    width: auto;
                    height: 100%;
                    margin-right: 8px;
                    display: flex;
                    flex-direction: column;
                    align-items: flex-start;
                    justify-content: flex-start;
                    padding: 8px;
                    font-size: 12px;
                  }
                  .error {
                    color: rgb(249, 57, 57);
                  }
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
  .monaco-editor .margin {
    width: 12px !important;
  }

  .flex-row {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: flex-start;
  }
</style>
  