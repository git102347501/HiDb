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


// 获取详情
export function getDbType(dbtype) {
    return request({
        url: '/table/db-type',
        method: 'post',
        headers: {
            'dbtype': dbtype
        }
    })
}

// 获取详情
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