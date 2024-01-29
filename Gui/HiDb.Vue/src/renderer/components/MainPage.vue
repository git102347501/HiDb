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
          <div style="margin-left: 8px;">
            <a-dropdown>
              <a-button type="text" style="color: #fff">关于</a-button>
              <template #overlay>
                <a-menu value="1" @click="selectedAbout">
                  <a-menu-item key="1">软件版本</a-menu-item>
                  <a-menu-item key="2">关于我们</a-menu-item>
                </a-menu>
              </template>
            </a-dropdown>
          </div>
        </div>
        <div class="title">
          <span class="info" @click="getLife">
            <a-tooltip title="点击重新尝试连接">
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
            </a-tooltip>
            <span style="margin-left: 10px;">|</span>
          </span>
          <span v-if="currDatabase.name" class="info">
            <sql-server v-if="currDatabase.type == 0"></sql-server>
            <my-sql v-if="currDatabase.type == 1" :color="'white'"></my-sql>
            <pg-sql v-if="currDatabase.type == 2"></pg-sql>
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
                  <template #icon="{ type, mode, database }">
                    <template v-if="type === 'db'">
                      <database-outlined />
                    </template>
                    <template v-if="type === 'mode'">
                      <tablet-outlined />
                    </template>
                    <template v-if="type === 'table-search'">
                      <a-input-search
                        placeholder="输入关键字搜索表" size="middle"  @search="onTableSearch($event, mode, database)"
                        :style="{ 'width': (menuWidth - 110) + 'px','max-width': '300px' }" 
                      />
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
                            <a-menu-item key="34">清空表</a-menu-item>
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
                      <a-button @click="clearEditData" :disabled="!currDatabase || !currDatabase.key" 
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
                  <a-select
                    :disabled="!currDatabase || !currDatabase.key" 
                    v-model:value="noPage"
                    style="width: 84px; margin-left: 4px;"
                  >
                    <a-select-option :value="true">不分页</a-select-option>
                    <a-select-option :value="false">分页</a-select-option>
                  </a-select>
                  <a-input-number v-if="!noPage"
                    style="margin-left: 4px;width: 84px;text-align: center;"
                    id="pageSize" v-model:value="pagination.pageSize" :min="1" />
                  <a-tooltip title="常用语句">
                    <a-button @click="openToolDrawerDialog" style="margin-left: 6px" type="default" shape="circle" :icon="h(BarsOutlined)" />
                  </a-tooltip>
                </div>
              </div>
              <div class="context" >
                <div class="sql" :style="{ 'height': editHeight + 'px' }">
                    <div ref="editorContainer" class="editor" style="height:100%; width: 100%;"></div>
                </div>
                <div :class="editHeight <= 105 ? 
                  'edit-drap-line edit-drap-line-left' : editHeight >= 850 ? 
                  'edit-drap-line edit-drap-line-right': 'edit-drap-line'" 
                  @mousedown="editResize"></div>
                <div class="data"  :style="{ 'height': editBodyHeight }" 
                    v-show="!currloading">
                    <a-table class="table"
                        v-if="isQuery && !errorMsg"
                        :columns="columns" 
                        size="small"
                        :data-source="currData"
                        :scroll="{ y: pageHeight, x: '(100% - ' + (menuWidth + 30) + ')'}"
                        :loading="loading"
                        :pagination="false"
                    >
                      <template #headerCell="{ column, text  }">
                        <a @click="tableHeardClick(column.title)">{{ column.title }}</a>
                      </template>
                      <template #bodyCell="{ column, text }">
                        <a @click="tableColumnClick(text)">{{ text }}</a>
                      </template>
                    </a-table>
                    <div class="msg-line">
                      <div class="msg error" v-if="errorMsg">
                        执行错误: {{errorMsg}}
                      </div>
                      <div class="msg" v-if="isQuery && !errorMsg && pagination.total != null &&  pagination.total != undefined">
                        总行数: {{ pagination.total }} ｜ 页行数: {{ pagination.pageSize }}
                      </div>
                      <div class="msg" v-if="!isQuery && !errorMsg">
                        影响行数: {{executeNum}}
                      </div>
                      <div class="msg" v-if="elapsedTimeRef">
                        执行耗时：{{ elapsedTimeRef }} ms
                      </div>
                    </div>
                </div>
                <div class="opt" v-show="loading">
                  <a-button @click="cancelQuery" type="dashed" danger>撤销查询</a-button>
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
      <a-modal v-model:open="openDbListDialog" title="数据库列表" width="830px" height="550px">
        <db-list-dialog  v-if="openDbListDialog" ref="dbListDialogRef" />
        <template #footer>
          <a-button key="submit" type="default" @click="selectDb(true)">打开</a-button>
          <a-button key="submit" type="primary" @click="selectDbAndOpen">连接</a-button>
        </template>
      </a-modal>
      <a-modal v-model:open="openDbDialog" title="连接数据库">
        <open-db-dialog v-if="openDbDialog" ref="openDbDialogRef" :model="selectDbVal"></open-db-dialog>
        <template #footer>
          <a-button key="back" @click="cancelDbDialog">取消</a-button>
          <a-button key="submit" type="primary" :loading="submitOpenDbLoading" @click="submitOpenDb">连接</a-button>
        </template>
      </a-modal>
      <a-modal v-model:open="openAboutDialog" width="680px" title="关于HiDb">
        <about-dialog></about-dialog>
        <template #footer>
          <a-button key="back" @click="openAboutDialog = false">关闭</a-button>
        </template>
      </a-modal>
      <a-modal v-model:open="openVersionDialog" width="480px" title="版本信息">
        <version-dialog></version-dialog>
        <template #footer>
          <a-button key="back" @click="openVersionDialog = false">关闭</a-button>
        </template>
      </a-modal>
      <a-drawer class="tools" :title="'常用语句 (' + currToolDrawerData.length  + ')'" :size="400" 
        :bodyStyle="{'padding': '8px'}" :open="openToolDrawer" @close="openToolDrawer = false">
        <template #extra>
          <a-tooltip title="添加语句">
            <a-button @click="addToolsSearch" type="default" shape="circle" :icon="h(PlusCircleOutlined)" />
          </a-tooltip>
          <a-tooltip title="保存全部">
            <a-button @click="saveToolsSearch" type="default" shape="circle" 
              style="margin: 6px 0 0 6px" :icon="h(SaveOutlined)" />
          </a-tooltip>
        </template>
        <div class="search">
          <a-input-search  v-model:value="toolSearchValue"
            placeholder="搜索语句"
            @search="onToolsSearch" />
        </div>
        <div class="list">
          <a-card size="small" hoverable
            v-for="(item, index) in currToolDrawerData"
            v-bind:key="index" style="width: 100%; margin-top: 6px;">
            <a-textarea v-model:value="item.data" placeholder="输入sql语句" :rows="4" />
            <a-tooltip title="删除">
              <a-button @click="deleteToolSearch(index)" danger shape="circle" 
              style="margin: 6px 6px 0 0" :icon="h(DeleteOutlined)" />
            </a-tooltip>
            <a-tooltip title="应用">
              <a-button @click="selectToolSearch(item.data)" type="default" shape="circle" 
                style="margin-top: 6px" :icon="h(CheckOutlined)" />
            </a-tooltip>
          </a-card>
          <a-empty description="暂无保存语句" v-if="!currToolDrawerData || currToolDrawerData.length < 1" />
        </div>
      </a-drawer>
    </div>
</template>

<script lang="ts" setup>

import { h, ref, watch, onMounted, UnwrapRef, reactive, createVNode, defineComponent  } from 'vue';
import { message, Modal } from 'ant-design-vue';
import { ExclamationCircleOutlined,BarsOutlined,PlusCircleOutlined,DeleteOutlined,CheckOutlined,SaveOutlined, WifiOutlined,ApiOutlined,UserOutlined,BorderlessTableOutlined,DatabaseOutlined,FileAddOutlined,CaretRightOutlined,RedoOutlined, DownOutlined, TabletOutlined, TableOutlined, FrownOutlined, FrownFilled  } from '@ant-design/icons-vue';
import { getDb,getMode,getTable } from '../api/menu';
import { getSearch,execute} from '../api/search';
import { connectDb } from '../api/datasource';
import type { MenuTheme,TreeProps,TableProps, MenuProps  } from 'ant-design-vue';
import { ConnectDbInput } from './model/MainPageMode';
import { DataType } from 'vue-request';
import { getGuid } from '@renderer/utils/guid';
import { life } from '../api/life';
import * as monaco from 'monaco-editor';
import MySql from './icons/MySql.vue';
import SqlServer from './icons/SqlServer.vue';
import PgSql from './icons/PgSql.vue';
import TableEdit from './edits/TableEdit.vue';
import AboutDialog from './dialogs/AboutDialog.vue';
import VersionDialog from './dialogs/VersionDialog.vue';
import OpenDbDialog from './dialogs/OpenDbDialog.vue';
import DbListDialog from './dialogs/DbListDialog.vue';
import { deleteTable, clearTable } from '../api/table';
import axios from 'axios';
import { getMaxLength } from '../utils/common';
import { dbTypeOptions } from '../utils/database';

  const sh = 280;
  const pageHeight = ref(0);
  const dftPageHeight = ref(0);
  const loading = ref(false);
  const openVersionDialog = ref(false);
  const editorContainer = ref<any>(null)
  let editor = null; // 当前编辑器
  const viewMode = ref(0); // 视图模式，0数据视图，1 编辑表视图 2编辑模式视图 3编辑数据库视图
  // 当前标记表
  const editTableData = ref({
    table: '',
    database: '',
    mode: '',
    dbtype: 0
  });
  const openToolDrawer = ref<boolean>(false); // 工具插窗
  const searchValue = ref<string>(''); // 左侧搜索内容
  const expandedMenuKeys = ref<string[]>([]); // tree搜索key
  const selectedMenuKeys = ref<string[]>([]); // tree选择key
  const executeNum = ref(0); // 影响行数
  const isQuery = ref(true); // 是否走查询
  const errorMsg = ref(''); // 错误消息
  const openAboutDialog = ref(false);
  // 选中数据库名称
  const currDbName = ref('');
  const submitOpenDbLoading = ref(false);
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
  const menuWidth =  ref(350); // 菜单宽度
  const bodyWidth =  ref('calc(100% - 350px)'); // 内容宽度
  const mouseEventX =  ref<number>(0);

  const editHeight =  ref(105); // 菜单宽度
  const editBodyHeight =  ref('calc(100% - 105px)'); // 内容宽度
  const mouseEventY =  ref<number>(0);

  // 查询模式
  const noPage = ref(false);
  const selectDbData = ref<Array<string>>([]);
  const currRightData = ref<any>(null);
  // 全局加载
  const currloading = ref<boolean>(false);
  // 后台存活监听
  const lifeTest = ref<number>(0);
  // 表格数据列
  const columns = ref<any[]>([]);
  const toolSearchValue = ref<string>('');
  // 主题
  const theme = ref<MenuTheme>('dark');
  // 当前表格数据
  const currData = ref<any[]>([]);
  // 表格分页信息
  const pagination = ref({
    total: null,
    pageSize: 100
  });
  const currToolDrawerData = ref<any[]>([]);
  const toolDrawerData = ref<any[]>([]);
  var cancelToken = axios.CancelToken.source();
  // 执行耗时/毫秒
  const elapsedTimeRef = ref<number | null>(0);

  // 创建时
  onMounted(() => {
    pageHeight.value = document.body.clientHeight - sh;
    dftPageHeight.value = pageHeight.value;
    window.addEventListener('resize', onResize);
    initEdit();
  });
  // 初始化编辑器
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
      triggerCharacters: ['@'],
      provideCompletionItems: (model, position) => {
        let suggestions = refCurrDbTableList();
        return {
          suggestions
        }
      }
    })
  }
  const openDbDialogRef = ref(null);
  const dbListDialogRef = ref(null);
  const callOpenDbInit = (data) => {
    console.log('callOpenDbInit');
    if (openDbDialogRef.value) {
      openDbDialogRef.value.init(data);
    }
  };

  const onToolsSearch = (val)=>{
    if (!val) {
      currToolDrawerData.value = toolDrawerData.value;
    } else {
      currToolDrawerData.value = toolDrawerData.value.filter(c=> c.data.includes(val));
    }
  }
  const addToolsSearch = (val) => {
    currToolDrawerData.value.unshift(val);
  }
  const saveToolsSearch = () => {
    if (toolSearchValue) {
      onToolsSearch('');
    }
    localStorage.setItem('toolsdata', JSON.stringify(currToolDrawerData.value));
    message.success('保存成功');
  }
  const deleteToolSearch = (index)=>{
    currToolDrawerData.value.splice(index, 1);
  }
  const selectToolSearch = (val)=>{
    editorAppendValue(val);
    openToolDrawer.value = false;
  }
  const initToolsSearch = ()=> {
    let data = localStorage.getItem('toolsdata');
    if (data) {
      toolDrawerData.value = JSON.parse(data);
      currToolDrawerData.value = toolDrawerData.value;
      console.log(JSON.stringify(currToolDrawerData.value));
    } else {
      toolDrawerData.value = [];
      currToolDrawerData.value = [];
    }
  }

  const openToolDrawerDialog = ()=>{
    initToolsSearch();
    openToolDrawer.value = true;
  }
  const onTableSearch = (data, mode, database)=>{
    console.log('onTableSearch');
    let currDb = treeData.value.find(c => c.title == database);
    if (!currDb || !currDb.children || currDb.children.length < 1) {
      return [];
    }
    let currMode = currDb.children.find(c => c.title == mode);
    if (!currMode || !currMode.children || currMode.children.length < 1) {
      return [];
    }
    if (!currMode.oldchildren) {
      currMode.oldchildren = currMode.children;
    }
    if (!data) {
      if (currMode.oldchildren){
        currMode.children = currMode.oldchildren;
        treeData.value = [...treeData.value];
        return;
      } else {
        return;
      }
    } 
    let currdata = data.toLowerCase();
    currMode.children = currMode.oldchildren.filter(c=> c.type == 'table-search' || c.title.toLowerCase().includes(currdata));
    if (currMode.children.length < 2) {
      currMode.children.push({            
        title: '未搜索到数据',
        key: '未搜索到数据',
        isLeaf: true,
        disabled: true,
        type: 'tablenull',
      });
    }
    treeData.value = [...treeData.value];
  }
  
  // 刷新当前数据库下拉表列表
  const refCurrDbTableList: any = () => {
    console.log('refCurrDbTableList');
    if (!currDbName.value || !treeData.value || treeData.value.length < 1) {
      return [];
    }
    let currDb = treeData.value.find(c => c.title == currDbName.value);
    if (!currDb || !currDb.children || currDb.children.length < 1) {
      return [];
    }
    let currMode = currDb.children[0];
    if (!currMode) {
      return [];
    }
    let children = currMode.oldchildren ? currMode.oldchildren : currMode.children;
    if (!children || children.length < 1) {
      return [];
    } else {
      return children.map((item: any) => {
        return {
          label: item.title,
          kind: item.title,
          insertText: item.title
        }
      });
    }
  }

  // 窗体大小改变事件
  const onResize = () => {
    pageHeight.value = document.body.clientHeight - sh;
    dftPageHeight.value = pageHeight.value;
    console.log('onResize:' + pageHeight.value);
  };

  // 菜单展开事件
  watch(currDbName, () => {
    console.log(currDbName.value);
    let currDb = treeData.value.find(c => c.title == currDbName.value);
    if (!currDb) {
      return;
    }
    if (!currDb.children || currDb.children.length < 1) {
      getMode(currDbName.value, currDatabase.value.type).then(res=>{
        if (!res.data || res.data.length < 1) {
          return [];
        }
        currDb.children = res.data.map(c => {
          return {            
            title: c.name,
            key: currDbName.value + '_' + c.name,
            isLeaf: false,
            type: 'mode',
            database: currDbName.value
          }
        });
        let currMode = currDb.children[0];
        getTable(currMode.database, currMode.title, currDatabase.value.type).then(res=>{
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
                mode: currMode.title,
                database: currMode.database
              }
            });
            currMode.children.unshift({
              title: '',
              key: currMode.title + '-s',
              isLeaf: true,
              style: {
                height: '35px'
              },
              type: 'table-search',
              mode: currMode.title,
              database: currMode.database
            });
          }
          treeData.value = [...treeData.value];
        });
      },() => {
        message.error('获取数据库信息失败');
      })
    } 
  });

  // 选中用户菜单
  const selectedUserMenu: MenuProps['onClick'] = ({ key }) => {
    if (key == '1') {
      clearCurrDbData();
    } 
  }
  
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

  const selectedAbout : MenuProps['onClick'] = ({ key }) => { 
    if (key == '1') {
      openVersionDialog.value = true;
    } else {
      openAboutDialog.value = true;
    }
  }

  const selectedMenu: MenuProps['onClick'] = ({ key }) => {
    if (key == '1') {
      submitOpenDbList();
    } else {
      selectDbVal.value = {
        key: getGuid(),
        name: '',
        account: '',
        passWord: '',
        address : '',
        port:  dbTypeOptions[0].port,
        type: 0,
        trustCert: true,
        trustedConnection: false,
        encrypt: true,
        saveLocal: true
      }
      openDbDialog.value = true;
    }
  };

  // 编辑器大小改变
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

  const selectDbfilterOption = (input: string, option: any) => {
    return option.value.toLowerCase().indexOf(input.toLowerCase()) >= 0;
  };
  const menuRightClick = (e)=>{
    currRightData.value = e.node;
  }
  

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
        editor.setValue( 'select * from `' + currRightData.value.title + '`');
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
      submitClearTable(currRightData.value.database, currRightData.value.title,
        currRightData.value.mode);
    }
  };
  // 清空当前数据库数据
  const clearCurrDbData = () => {
    clearEditData();
    treeData.value = [];
    currData.value = [];
    expandedMenuKeys.value = [];
    selectedMenuKeys.value = [];
    searchValue.value = '';
    isQuery.value = true;
    pagination.value.total = undefined;
    pagination.value.pageSize = 100;
    currDbName.value = '';
    selectDbData.value = [];
    elapsedTimeRef.value = null;
    errorMsg.value = '';
    noPage.value = false;
    viewMode.value = 0;
    columns.value = [];
    selectDbVal.value = {
      key: getGuid(),
      name: '',
      account: '',
      passWord: '',
      address : '',
      port:  dbTypeOptions[0].port,
      type: 0,
      trustCert: true,
      trustedConnection: false,
      encrypt: true,
      saveLocal: true
    }
  }
  // 打开数据库连接
  const submitOpenDb = ()=> {
    currloading.value = true;
    submitOpenDbLoading.value = true;
    let data = openDbDialogRef.value && openDbDialogRef.value.openDbModel ?  openDbDialogRef.value.openDbModel : selectDbVal.value;
    connectDb(data).then(res=>{
      currloading.value = false;
      if (!res.data || !res.data || !res.data.success) {
        message.error(res.data.message);
        submitOpenDbLoading.value = false;
      } else {
        message.success('连接成功');
        openDbDialog.value = false;
        submitOpenDbLoading.value = false;
        currDatabase.value = data;
        // 保存到本地 warning
        // saveDbByLocal(data);
        // 加载数据库列表
        loadDataBase();
      }
    }, err => { 
      currloading.value = false;
      submitOpenDbLoading.value = false;
      message.error(err && err.message ? err.message : '连接失败');
    })
  };
  // 保存数据库到本地
  const saveDbByLocal = (data) => {
    // 寻找相同地址，账号和类型的本地记录
    let currdbData = dbListDialogRef.value.currdbData;
    let index = currdbData.findIndex(c=> c.key == data.key);
    if (!data.name || data.name.length < 1) {
      // 默认名称为地址
      data.name = data.address;
    }
    if (data.saveLocal){
      if (index != -1) {
        // 更新本地
        currdbData[index].passWord = data.passWord;
        currdbData[index].port = data.port;
        currdbData[index].trustCert = data.trustCert;
        currdbData[index].trustedConnection = data.trustedConnection;
        currdbData[index].encrypt = data.encrypt;
      } else {
        // 新增本地
        currdbData.push(data);
      }
    } else {
      if (index == -1) {
        // 不保存本地也没有，跳过
        return;
      } else {
        // 不保存移除本地
        currdbData.splice(index, 1);
      }
    }
    console.log('save-local:' + JSON.stringify(currdbData));
    localStorage.setItem('hidbdata', JSON.stringify(currdbData));
  }
  // 打开数据库列表
  const submitOpenDbList = () => {
    openDbListDialog.value = true;
  }
  // 选择db
  let selectDbVal = ref<ConnectDbInput>(null);
  const selectDb = (openDialog)=> {
    console.log(dbListDialogRef.value);
    let currSelectDb = dbListDialogRef.value.currSelectDb;
    if (!currSelectDb || !currSelectDb.key) {
      message.error('请选择一个数据库');
      return false;
    }
    selectDbVal.value = currSelectDb;
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
  // 树目录选择事件
  const onSelect = (selectedKeys, e)=>{
    console.log('onSelect');
    console.log(e);

    if (e && e.node) {
      if (e.node.dataRef.type === 'table') {
        if (currDatabase.value.type == 0) {
          editor.setValue('select * from ' + e.node.dataRef.title);
        } else if (currDatabase.value.type == 1) {
          editor.setValue( 'select * from `' + e.node.dataRef.title + '`');
        }
        currDbName.value = e.node.dataRef.database;
        console.log('currDbName' + currDbName.value);
      }
    }
  }
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

  // 清空
  const clearEditData = () => {
    editor.setValue('');
  }

  // 获取是否为查询语句
  const getIsQuery = (val) => {
    const notquery = /(update|delete|drop|alter|truncate)/i;
    if (notquery.test(val)) {
      return false;
    }
    const query = /(select|show)/i;
    if (query.test(val)) {
      return true;
    }
    return false;
  }

  // 清空查询消息
  const clearMsg = ()=>{
    errorMsg.value = '';
  }

  // 表格主查询
  const searchData = () => {
    cancelToken = axios.CancelToken.source();
    let sql = getSelectedText();
    if (!sql) {
      message.error('执行语句不能为空!');
      return;
    }
    
    clearMsg();

    if (!getIsQuery((sql as any).toLowerCase())) {
      isQuery.value = false;
      loading.value = true;
      execute({
        database: currDbName.value ? currDbName.value : '',
        sql: sql
      }, currDatabase.value.type, cancelToken.token).then(res => {
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
        pageSize: pagination.value.pageSize,
        noPage: noPage.value
      }, currDatabase.value.type, cancelToken.token).then(res => {
        loading.value = false;
        elapsedTimeRef.value = res.data.elapsedTime;
        errorMsg.value = res.data.message;
        console.log(res);
        if (res && res.data && res.data.list && res.data.list.length > 0) {
          let obj  = res.data.list[0];
          columns.value = Object.keys(obj).map((element, index) => ({
            title: element ? element : 'filed' + (index + 1),
            dataIndex:  element ? element : 'filed' + (index + 1),
            sorter: false,
            width: 30 + (getMaxLength(res.data.list, element) * 10)
          }));
          res.data.list.forEach((obj, index) => {
            Object.keys(obj).forEach((key, index) => {
              // 检查属性是否为空
              if (key === "") {
                const newKey = "filed" + (index + 1);
                obj[newKey] = obj[key];
                delete obj[key]; // 删除原属性
              }
            });
          });
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

  // 撤销查询
  const cancelQuery = ()=>{
    cancelToken.cancel('您已撤销查询');
  }

  const tableHeardClick = (val)=>{
    editorAppendValue(val);
  }
  const tableColumnClick = (val)=>{
    editorAppendValue(val);
  }
  const editorAppendValue = (text)=>{
    // 获取焦点位置
    const position = editor.getPosition(); 
    const appendText = {
      range: new monaco.Range(position.lineNumber, position.column, position.lineNumber, position.column),
      text: text,
      forceMoveMarkers: true
    };

    // 执行编辑操作
    editor.executeEdits('appendText', [appendText]);
  }

  // 提交删除表
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
        });
      },
    });
  }
  const submitClearTable = (database, table, mode)=>{
    Modal.confirm({
      title: '确认要清空表[' + table + ']数据吗？',
      icon: createVNode(ExclamationCircleOutlined),
      content: '请谨慎操作，删除的数据不可恢复',
      okText: '确认',
      cancelText: '取消',
      onOk() {
        return new Promise((resolve, reject) => {
          clearTable(currDatabase.value.type, database, mode, table).then(dres=>{
            if (dres) {
              message.success('清空成功');
            } else {
              message.warning('清空失败');
            }
            resolve(true);
          }, () => {
            message.error('清空错误');
            reject(false)
          })
        });
      },
    });
  }
</script>
<style lang="scss" scoped src="./MainPage.scss"></style>