<template>
    <a-layout class="main">
      <a-layout-header class="header">
        <div class="width: 100%; height: 100%">
            <a-menu
            v-model:openKeys="openKeys"
            v-model:selectedKeys="selectedKeys"
            style="width: 200px; border-radius: 0px;"
            mode="inline"
            :theme="theme"
            :items="items"
            />
        </div>
      </a-layout-header>
      <a-layout>
        <a-layout-sider width="200" class="menu">
          <div class="search">
            <a-input-search v-model:value="searchValue" style="margin-bottom: 8px" placeholder="Search" />
          </div>
          <div class="tree">
            <a-tree style="height: 100%;"
                v-model:expandedKeys="expandedKeys"
                v-model:selectedKeys="selectedMenuKeys"
                :load-data="onLoadData"
                :tree-data="treeData"
            />
          </div>
        </a-layout-sider>
        <a-layout class="work" style="padding: 0 8px 8px">
          <div class="tools">
            <a-tooltip title="执行">
                <a-button :icon="h(CaretRightOutlined)" style="margin-right: 6px;">执行</a-button>
            </a-tooltip>
            <a-tooltip title="撤销">
                <a-button :icon="h(RedoOutlined)">撤销</a-button>
            </a-tooltip>
          </div>
          <a-layout-content  class="content"
            :style="{ background: '#fff', padding: '6px', margin: 0, minHeight: '280px' }"
          >
            <div class="sql">
                <a-textarea class="input"
                    v-model:value="optValue"
                    placeholder="输入SQL语句后执行"
                    :auto-size="{ minRows: 4, maxRows: 15 }" />
            </div>
            <div class="data">
                <a-table
                    :columns="columns" size="small"
                    :row-key="record => record.login.uuid"
                    :data-source="dataSource"
                    :pagination="pagination"
                    :loading="loading"
                    @change="handleTableChange"
                >
                    <template #bodyCell="{ column, text }">
                    <template v-if="column.dataIndex === 'name'">{{ text.first }} {{ text.last }}</template>
                    </template>
                </a-table>
            </div>
          </a-layout-content>
        </a-layout>
      </a-layout>
    </a-layout>
  </template>
  <script lang="ts" setup>
  import { h, ref, watch, computed } from 'vue';
  import { usePagination } from 'vue-request';
  import axios from 'axios';
  import type { MenuTheme,TreeProps,TableProps  } from 'ant-design-vue';
  const optValue = ref<string>('');
    const searchValue = ref<string>('');
    const expandedKeys = ref<string[]>([]);
    const selectedMenuKeys = ref<string[]>([]);
    const treeData = ref<TreeProps['treeData']>([
        { title: 'database1', key: '0' },
        { title: 'database2', key: '1' },
        { title: 'database3', key: '2', isLeaf: true },
    ]);
    const onLoadData: TreeProps['loadData'] = treeNode => {
    return new Promise<void>(resolve => {
    if (treeNode.dataRef.children) {
      resolve();
      return;
    }
    setTimeout(() => {
      treeNode.dataRef.children = [
        { title: 'dbo', key: `${treeNode.eventKey}-0` },
        { title: 'master', key: `${treeNode.eventKey}-1` },
      ];
      treeData.value = [...treeData.value];
      resolve();
    }, 1000);
  });
};const dataList: TreeProps['treeData'] = [];
  const selectedKeys1 = ref<string[]>(['2']);
  const selectedKeys2 = ref<string[]>(['1']);
    watch(searchValue, value => {
  const expanded = dataList
    .map((item: TreeProps['treeData'][number]) => {
      if (item.title.indexOf(value) > -1) {
        return getParentKey(item.key, gData.value);
      }
      return null;
    })
    .filter((item, i, self) => item && self.indexOf(item) === i);
        expandedKeys.value = expanded;
        searchValue.value = value;
        autoExpandParent.value = true;
    });
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
    const columns = [
  {
    title: 'Name',
    dataIndex: 'name',
    sorter: true,
    width: '20%',
  },
  {
    title: 'Gender',
    dataIndex: 'gender',
    filters: [
      { text: 'Male', value: 'male' },
      { text: 'Female', value: 'female' },
    ],
    width: '20%',
  },
  {
    title: 'Email',
    dataIndex: 'email',
  },
];
  const openKeys = ref<string[]>(['sub1']);
  const selectedKeys = ref(['1']);
  import { AppstoreOutlined,DatabaseOutlined,FileAddOutlined,CaretRightOutlined,RedoOutlined } from '@ant-design/icons-vue';
  const theme = ref<MenuTheme>('dark');
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
const queryData = (params: APIParams) => {
  return axios.get<APIResult>('https://randomuser.me/api?noinfo', { params });
};
const {
  data: dataSource,
  run,
  loading,
  current,
  pageSize,
} = usePagination(queryData, {
  formatResult: res => res.data.results,
  pagination: {
    currentKey: 'page',
    pageSizeKey: 'results',
  },
});

const pagination = computed(() => ({
  total: 200,
  current: current.value,
  pageSize: pageSize.value,
}));

const handleTableChange: TableProps['onChange'] = (
  pag: { pageSize: number; current: number },
  filters: any,
  sorter: any,
) => {
  run({
    results: pag.pageSize,
    page: pag?.current,
    sortField: sorter.field,
    sortOrder: sorter.order,
    ...filters,
  });
};
type APIParams = {
  results: number;
  page?: number;
  sortField?: string;
  sortOrder?: number;
  [key: string]: any;
};
type APIResult = {
  results: {
    gender: 'female' | 'male';
    name: string;
    email: string;
  }[];
};
  </script>
<style lang="scss" scoped>
  #components-layout-demo-top-side-2 .logo {
    float: left;
    width: 120px;
    height: 31px;
    margin: 16px 24px 16px 0;
    background: rgba(255, 255, 255, 0.3);
  }
  
  .ant-row-rtl #components-layout-demo-top-side-2 .logo {
    float: right;
    margin: 16px 0 16px 24px;
  }
  
  .site-layout-background {
    background: #fff;
  }
  .main {
    margin-top: 30px;
    .header {
        height: 50px;
        width: 100%;
        margin: 0;
        padding: 0;
        z-index: 999;
    }
    .menu {
        width: 200px;
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
            height: calc(100% - 45px);
        }
    }
    .work {
        height: calc(100vh - 120px);
        width: 100%;    
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
                min-height: 100px;
                .input {
                    height: calc(100% - 8px);
                    margin: 4px;
                    width: calc(100% - 8px);
                }
            }
            .data {
                width: 100%;
            }
        }
    }
  }
  </style>
  