// 操作防抖器
export function debounce (fn: Function, delay: number, args: any){
    let timer: any = null;

    if (timer) {
        clearTimeout(timer);//如果方法多次触发，则把上次记录的延迟执行代码清掉，重新开始
    }
    timer = setTimeout(() => {
        fn(args);
    }, delay);
}

// 自动列宽计算
export function getMaxLength (objCollection, name){
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

export function isNull(val){
    return val == null || val == undefined;
}