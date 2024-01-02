import { getDbType } from '../../api/table';
import { props, dbTypeOptions } from './MySqlEdit.vue';

export const loadDbType = () => {
getDbType(props.dbtype).then(res => {
dbTypeOptions.value = res.data.map(c => { return { title: c.name }; });
});
};
