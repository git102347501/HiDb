<template>
    <a-layout class="main">
      <a-layout-header class="header">
        <div class="btn">
          <a-dropdown>
            <a-button ghost>操作</a-button>
            <template #overlay>
              <a-menu >
                <a-menu-item key="1">数据库列表</a-menu-item>
                <a-menu-item key="2">新的连接</a-menu-item>
              </a-menu>
            </template>
          </a-dropdown>
        </div>
      </a-layout-header>
      <a-layout>
        <a-layout-sider width="200" class="menu">
          <div class="search">
            <a-input-search v-model:value="searchValue" style="margin-bottom: 8px" placeholder="Search"  @search="onSearch" />
          </div>
          <div class="tree">
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
          </div>
        </a-layout-sider>
        <a-layout class="work" style="padding: 0 8px 8px">
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
                  总记录行数:{{ pagination.total }}
                </div>
            </div>
          </a-layout-content>
        </a-layout>
      </a-layout>
      <a-modal v-model:open="openDbDialog" title="Basic Modal" @ok="handleOk">
        <div>
          <a-form :model="formState" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-form-item label="Activity name">
              <a-input v-model:value="formState.name" />
            </a-form-item>
          </a-form>
        </div>
        <template #footer>
          <a-button key="back" @click="cancelDbDialog">取消</a-button>
          <a-button key="submit" type="primary" :loading="loading" @click="handleOk">连接</a-button>
        </template>
      </a-modal>
    </a-layout>
</template>

<script lang="ts" setup>
import { h, ref, watch, onMounted  } from 'vue';
import { AppstoreOutlined,DatabaseOutlined,FileAddOutlined,CaretRightOutlined,RedoOutlined, DownOutlined, TabletOutlined, TableOutlined, FrownOutlined, FrownFilled  } from '@ant-design/icons-vue';
import { getDb,getMode,getTable } from '../api/menu';
import { getSearch,execute} from '../api/search';
import type { MenuTheme,TreeProps,TableProps  } from 'ant-design-vue';
import { message } from 'ant-design-vue';

  const sh = 320;
  const pageHeight = ref(0);
  const loading = ref(false);
  onMounted(() => {
    pageHeight.value = document.body.clientHeight - sh;
    console.log('onMounted:' + pageHeight.value);
    window.addEventListener('resize', onResize)
  });
  const onResize = () => {
    pageHeight.value = document.body.clientHeight - sh;
    console.log('onResize:' + pageHeight.value);
  };
  const [messageApi] = message.useMessage(); // 消息
  const optValue = ref<string>(''); // 执行SQL
  const searchValue = ref<string>(''); // 左侧搜索内容
  const expandedMenuKeys = ref<string[]>([]); // tree搜索key
  const selectedMenuKeys = ref<string[]>([]); // tree选择key
  const executeNum = ref(0); // 影响行数
  const isQuery = ref(true); // 是否走查询
  // tree数据
  const treeData = ref<TreeProps['treeData']>([
  ]);
  const openDbDialog = ref<boolean>(false);

  // 加载数据库列表
  const loadDataBase = ()=>{
    getDb().then(res => {
      console.log(res);
      treeData.value = res.data.map(c=> {
        return {
          title: c.name,
          key: c.name,
          isLeaf: false,
          type: 'db'
        }});
      console.log(treeData.value);
    })
  }
  // 默认加载列表
  loadDataBase();

  // 搜索
  const onSearch = (searchValue: string) => {
    loadDataBase();
  };
  const cancelDbDialog = ()=>{
    openDbDialog.value = false;
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
            getMode(treeNode.title).then(res=>{
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
            })
          } else if (treeNode.dataRef.type === 'mode'){
            getTable(treeNode.dataRef.database,treeNode.title).then(res=>{
              treeNode.dataRef.children = res.data.map(c => {
                return {            
                  title: c.name,
                  key: c.name,
                  isLeaf: true,
                  type: 'table'
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
      if (e && e.node && e.node.dataRef.type === 'table') {
        optValue.value = 'select * from ' + e.node.dataRef.title;
        currDbName.value = e.node.dataRef.database;
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
    // 主题
    const theme = ref<MenuTheme>('dark');
    // 当前表格数据
    const currData = ref<any[]>([]);
    // 表格分页信息
    const pagination = ref({
      total: null,
      pageSize: 1000
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

    // 表格主查询
    const searchData = () => {
      let sql = getSelectedText();
      if (!sql) {
        messageApi.error('执行语句不能为空!');
        return;
      }
      if ((sql as any).toLowerCase().indexOf('select') == -1) {
        isQuery.value = false;
        loading.value = true;
        execute({
          database: currDbName.value ? currDbName.value : '',
          sql: sql
        }).then(res => {
          loading.value = false;
          executeNum.value = res.data;
        }, err => {
          loading.value = false;
          messageApi.error(err);
        })
      } else {
        isQuery.value = true;
        loading.value = true;
        getSearch({
          database: currDbName.value ? currDbName.value : '',
          sql: sql,
          pageSize: pagination.value.pageSize
        }).then(res => {
          loading.value = false;
          console.log(res);
          if (res && res.data && res.data.list && res.data.list.length > 0) {
            let obj  = res.data.list[0];
            columns.value = Object.keys(obj).map(key => ({
              title: key,
              dataIndex: key,
              sorter: false,
              width: 40 + (key.length * 10)
            }));
            currData.value = res.data.list;
            pagination.value.total = res.data.count;
          } else {
            currData.value = [];
            pagination.value.total = 0;
          }
        }, err => {
          loading.value = false;
          messageApi.error(err);
        })
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

        .btn {
          width: calc(100% - 12px);
          height: 100%;
          display: flex;
          flex-direction: row;
          align-items: center;
          justify-content: flex-start;
          padding: 0 6px;
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
</style>
  