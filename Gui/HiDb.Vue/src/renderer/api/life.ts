import request from '@renderer/utils/request'

// 测试连接
export function life() {
    return request({
      url: '/life',
      method: 'get'
    })
}