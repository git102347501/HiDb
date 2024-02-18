import request from '@renderer/utils/request'

// 获取列表
export function getTableColumnList(data, dbtype) {
    return request({
        url: '/table/column/list',
        method: 'post',
        data: data,
        headers: {
            'dbtype': dbtype
        }
    })
}

// 获取详情
export function getTableColumnDetail(data, dbtype) {
    return request({
        url: '/table/column/detail',
        method: 'post',
        data: data,
        headers: {
            'dbtype': dbtype
        }
    })
}


// 获取数据库类型列表
export function getDbType(dbtype) {
    return request({
        url: '/table/db-type',
        method: 'post',
        headers: {
            'dbtype': dbtype
        }
    })
}

// 删除表格]
export function deleteTable(dbtype, database, mode, table) {
    return request({
        url: '/table',
        method: 'delete',
        headers: {
            'dbtype': dbtype
        },
        params: {
            database,
            table,
            mode
        }
    })
}

// 清空表
export function clearTable(dbtype, database, mode, table) {
    return request({
        url: '/table/clear',
        method: 'put',
        headers: {
            'dbtype': dbtype
        },
        params: {
            database,
            table,
            mode
        }
    })
}

// 更新指定列配置
export function updateTableColumn(data, dbtype) {
    return request({
        url: '/table/column/config',
        method: 'put',
        headers: {
            'dbtype': dbtype
        },
        data: data
    })
}

// 添加指定列配置
export function addTableColumn(data, dbtype) {
    return request({
        url: '/table/column/config',
        method: 'post',
        headers: {
            'dbtype': dbtype
        },
        data: data
    })
}

// 删除指定列配置
export function deleteTableColumn(dbtype, database, mode, table, column) {
    return request({
        url: '/table/column/config',
        method: 'delete',
        headers: {
            'dbtype': dbtype
        },
        data: {
            database,
            mode,
            table,
            column
        }
    })
}