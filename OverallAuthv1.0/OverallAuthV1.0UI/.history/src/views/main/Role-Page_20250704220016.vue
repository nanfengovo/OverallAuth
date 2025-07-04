<template>
    <el-card shadow="hover" class="search-card">
        <el-form label-width="80px" :model="searchForm" ref="formRef">
            <el-row :gutter="20" ref="formRef" justify="center">
                <el-col :span="8">
                    <el-form-item label="角色名:" prop="name">
                        <el-input type="text" v-model="searchForm.name" placeholder="请输入需要查询的角色名:" />
                    </el-form-item>
                </el-col>
                <el-col :span="8">
                    <el-form-item label="菜单:" prop="menu">
                        <el-input type="text" v-model="searchForm.menu" placeholder="请输入包含的菜单:" />
                    </el-form-item>
                </el-col>
                <el-col :span="8">
                    <el-button type="primary" icon="search" @click="headleSearchClick">
                        查询
                    </el-button>
                    <el-button icon="Refresh" @click="headleResetClick">
                        重置
                    </el-button>
                </el-col>
            </el-row>
        </el-form>
    </el-card>
    <div class="role-content">
        <div class="content-top">
            <div class="title">
                <el-button type="primary" icon="Refresh" @click="refresh">刷新</el-button>
                <el-button type="danger" icon="delete" @click="headleDeleteClick">删除</el-button>
            </div>
            <el-button icon="Plus" type="primary" @click="headleAddClick">新增角色</el-button>
        </div>
        <div class="content" v-loading="loading">
            <el-scrollbar max-height="550px">
                <el-table :data=data border style="width: auto;" stripe ref="multipleTableRef">

                    <el-table-column align="center" type="selection" width="40px" />
                    <el-table-column align="center" type="index" label="序号" width="60px" />
                    <el-table-column align="center" label="操作" width="130">
                        <template #default="scope">
                            <el-button type="primary" size="small" text icon="Edit"
                                @click="Editdialog(scope.row)">编辑</el-button>
                            <!-- <el-button :type="scope.row.isOpen ? 'danger' : 'primary'" size="small" :text="true"
                                :icon="scope.row.isOpen ? 'CircleCloseFilled' : 'SuccessFilled'">
                                {{ scope.row.isOpen ? '禁用' : '启用' }}
                            </el-button> -->
                        </template>
                    </el-table-column>
                    <el-table-column align="center" prop="name" label="角色名" width="180" />
                    <el-table-column align="center" prop="menusName" label="菜单" width="280" />

                    <el-table-column align="center" prop="describe" label="描述" width="180" />
                    <el-table-column align="center" prop="isEnable" label="是否启用" width="100">
                        <template #default="scope">
                            <el-switch v-model="scope.row.isEnable" disabled active-color="#13ce66"
                                inactive-color="#ff4949" :type="scope.row.isEnable ? 1 : 0" size="small">
                                {{ scope.row.isEnable ? '启用' : '禁用' }}
                            </el-switch>
                        </template>
                    </el-table-column>

                    <el-table-column align="center" prop="createTime" label="创建时间" width="180" />
                    <el-table-column align="center" prop="updateTime" label="更新时间" width="180" />


                </el-table>
            </el-scrollbar>
        </div>


    </div>

    <!-- 添加角色对话框 -->
    <el-dialog v-model="dialogVisible" title="添加角色" width="30%" draggable center>
        <el-form :model="Dialogform" label-width="120px">
            <el-form-item label="角色名">
                <el-input v-model="Dialogform.name" />
            </el-form-item>
            <!-- <el-form-item label="菜单">
                <el-tree style="max-width: 600px" :props="props" v-model="menusdata" lazy show-checkbox
                    @check-change="handleCheckChange" />
            </el-form-item> -->
            <el-form-item label="菜单">
                <el-tree :data="menusdata" :props="props" node-key="id" show-checkbox lazy
                    @check-change="handleCheckChange">
                    <template #default="{ node, data }">
                        <span class="custom-tree-node">
                            <el-icon>
                                <component :is="data.icon" /> <!-- 关键：动态组件绑定 -->
                            </el-icon>
                            <i :class="`el-icon-${data.icon}`" style="color: #2482ED; margin-right: 8px;"></i>
                            <span>{{ node.label }}</span>
                        </span>
                    </template>
                </el-tree>
            </el-form-item>
            <el-form-item label="描述">
                <el-input v-model="Dialogform.describe" type="textarea" />
            </el-form-item>
            <el-form-item label="是否启用">
                <el-switch v-model="Dialogform.isEnable" active-color="#13ce66" inactive-color="#ff4949" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="dialogVisible = false">取消</el-button>
                <el-button type="primary" @click="AddRole">确定</el-button>
            </span>
        </template>
    </el-dialog>



    <!-- 编辑角色对话框 -->
    <el-dialog v-model="dialogEditVisible" title="添加角色" width="30%" draggable center>
        <el-form :model="DialogEditform" label-width="120px">
            <el-form-item label="角色名">
                <el-input v-model="DialogEditform.name" />
            </el-form-item>
            <!-- <el-form-item label="菜单">
                <el-tree style="max-width: 600px" :props="props" v-model="menusdata" lazy show-checkbox
                    @check-change="handleCheckChange" />
            </el-form-item> -->
            <el-form-item label="菜单">
                <el-tree :data="menusdata" :props="props" node-key="id" show-checkbox lazy
                    :default-checked-keys="DialogEditform.checkedKeys" @check-change="handleCheckChange">
                    <template #default="{ node, data }">
                        <span class="custom-tree-node">
                            <el-icon>
                                <component :is="data.icon" /> <!-- 关键：动态组件绑定 -->
                            </el-icon>
                            <i :class="`el-icon-${data.icon}`" style="color: #2482ED; margin-right: 8px;"></i>
                            <span>{{ node.label }}</span>
                        </span>
                    </template>
                </el-tree>
            </el-form-item>
            <el-form-item label="描述">
                <el-input v-model="DialogEditform.describe" type="textarea" />
            </el-form-item>
            <el-form-item label="是否启用">
                <el-switch v-model="DialogEditform.isEnable" active-color="#13ce66" inactive-color="#ff4949" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="dialogEditVisible = false">取消</el-button>
                <el-button type="primary" @click="EditRole">确定</el-button>
            </span>
        </template>
    </el-dialog>

</template>
<script setup lang="ts">
// import { ref } from 'vue'

// const data = [{
//     role: '管理员',
//     Permissions: '系统管理,用户管理,角色管理'
// }
// ]

import { ElMessageBox, ElMessage, type ElForm, ElTable } from 'element-plus';
// import type { LoadFunction } from 'element-plus'
import { reactive, ref, nextTick } from 'vue';
import { onMounted } from 'vue';
import axios from 'axios';

// import fetchMenuData from '@/views/main/MenuManage-Page.vue'

const isMounted = ref(true);
const loading = ref(false)
//#region ---刷新

const refresh = () => {
    isMounted.value = false;
    setTimeout(() => {
        fetchRoleData();
        loading.value = false; //0.5 后显示内容
    }, 500);
    loading.value = true;
    nextTick(() => {
        isMounted.value = true;
    });
}
//#endregion









//#region   重置
const formRef = ref<InstanceType<typeof ElForm>>()
const searchForm = reactive({
    'name': '',
    'menu': ''
})


//重置
function headleResetClick() {
    formRef.value?.resetFields();
    fetchRoleData();
}
//#endregion

//#region 搜索
const headleSearchClick = async () => {
    if (searchForm.name === '' && searchForm.menu === '') {
        ElMessage.warning('请输入查询条件');
        return;
    }
    try {
        const res = await axios.post("http://127.0.0.1:5141/api/Role/Search", {
            name: searchForm.name,
            menu: searchForm.menu
        });
        if (res.data.code === 200) {
            data.value = res.data.data.map((item: Role) => ({
                name: item.name,
                menusName: item.menusName,
                describe: item.describe || '',
                isEnable: item.isEnable,
                createTime: item.createTime,
                updateTime: item.updateTime
            }));
        } else {
            ElMessage.error('获取角色数据失败:', res.data.message);
        }
    } catch (error) {
        ElMessage.error('获取角色数据失败:', error);
    }
}
//#endregion

//#region 删除
const multipleTableRef = ref<InstanceType<typeof ElTable>>(); // 明确组件类型
const headleDeleteClick = async (row: Role) => {
    try {
        await ElMessageBox.confirm('确定删除选中项吗？', '警告', { type: 'warning' });
        try {
            if (!multipleTableRef.value) {
                ElMessage.warning('表格未加载');
                return;
            }
            // 获取选中的行
            const selectedRows = multipleTableRef.value.getSelectionRows();
            const ids = selectedRows.map((row: { id: any; }) => row.id);
            const res = await axios.delete("http://127.0.0.1:5141/api/Role/DeleteRole", { data: ids });
            if (res.data.code === 200) {
                ElMessage.success('删除成功');
                refresh();
            } else {
                ElMessage.error('删除失败');
            }
        }
        catch (error) {
            ElMessage.error('删除失败，系统异常，请稍后再试' + error);
            return;
        }
    } catch (error) {

    }
}
//#endregion


onMounted(() => {
    fetchRoleData();
})

interface Role {
    id: number;
    name: string;
    menusName: string[];
    describe: string;
    isEnable: boolean;
    createTime?: string;
    updateTime?: string;
}

const data = ref<Role[]>([]);

const fetchRoleData = async () => {
    try {
        const res = await axios.get("http://127.0.0.1:5141/api/OverallAuth/GetAllRole");
        if (res.data.code === 200) {
            data.value = res.data.data.map((item: Role) => ({
                id: item.id,
                name: item.name,
                menusName: item.menusName,
                describe: item.describe || '',
                isEnable: item.isEnable,
                createTime: item.createTime,
                updateTime: item.updateTime
            }));
        } else {
            ElMessage.error('获取角色数据失败:', res.data.message);
        }
    } catch (error) {
        ElMessageS.error('获取角色数据失败:', error);
    }
}

//#region 新增角色
interface menu {
    id: number;
    name: string;
    icon: string;
}
const menusdata = ref<menu[]>([]);
const fetchMenuData = async () => {
    try {
        const res = await axios.get('http://127.0.0.1:5141/api/Menu/GetAllMenu');
        if (res.data.code === 200) {
            menusdata.value = res.data.data.map((item: menu) => ({
                id: item.id,
                name: item.name,
                icon: item.icon,

            }));
            console.log('菜单数据:', menusdata.value);
        } else {
            console.error('获取菜单数据失败:', res.data.message);
        }
    }
    catch (error) {
        console.error('获取菜单数据失败:', error);
    }
}



const dialogVisible = ref(false);

interface DialogForm {
    name: string;
    menuids: number[];
    describe: string;
    isEnable: boolean;
}

const Dialogform = reactive<DialogForm>({
    name: '',
    menuids: [],
    describe: '',
    isEnable: true
})

interface Tree {
    name: string
}
// let count = 1
const props = {
    label: 'name',
    children: 'zones',
}

const handleCheckChange = (
    data: Tree,
    checked: boolean,
    //indeterminate: boolean
) => {
    if (checked) {
        Dialogform.menuids.push(data.id)
    }
}

const AddRole = async () => {
    try {
        if (Dialogform.name === '' || Dialogform.describe === '') {
            ElMessage.warning('角色名和描述是必填项!!');
        } else {
            const res = await axios.post("http://127.0.0.1:5141/api/Role/AddRole", {
                name: Dialogform.name,
                menuids: Dialogform.menuids,
                describe: Dialogform.describe,
                isEnable: Dialogform.isEnable
            })
            if (res.data.code === 200) {
                ElMessage.success(res.data.msg);
                fetchRoleData();
                Dialogform.name = '';
                Dialogform.menuids = [];
                Dialogform.describe = '';
                dialogVisible.value = false;
            }
            else {
                ElMessage.error(res.data.msg);
                Dialogform.name = '';
                Dialogform.menuids = [];
                Dialogform.describe = '';
                dialogVisible.value = false;
            }


        }

    }
    catch (error) {

    }
}

const headleAddClick = async () => {
    dialogVisible.value = true;
    fetchMenuData();



}
//#endregion


//#region 编辑角色
const dialogEditVisible = ref(false);


const DialogEditform = ref({
    name: '',
    menus: [''],
    describe: '',
    isEnable: true
})
const Editdialog = async (row: Role) => {
    dialogEditVisible.value = true;
    DialogEditform.value.name = row.name;
    DialogEditform.value.menus = row.menusName;
    DialogEditform.value.describe = row.describe;
    DialogEditform.value.isEnable = row.isEnable;
    await fetchMenuData();
    menusdata.value.forEach(element => {
        DialogEditform.value.menus.forEach(item => {
            if (element.name === item) {
                element.checked = true;
            }
        })
    });
    // 提取需要默认选中的节点 ID
    const checkedKeys = menusdata.value
        .filter(menu => row.menusName.includes(menu.name))
        .map(menu => menu.id);
    // 绑定到 el-tree
    DialogEditform.value.checkedKeys = checkedKeys; // 用于 :default-checked-keys
    console.log(row.menusName);
}




//#endregion
</script>

<style lang="less" scoped>
.role-content {
    margin-top: 20px;
    background-color: #fff;
    border-radius: 20px;
}

.content-top {
    display: flex;
    justify-content: space-between;
    padding: 20px;
    align-items: flex-end;
    margin-bottom: 15px;

    .title {
        display: flex;
        align-items: center;

        h2 {
            font-size: 18px;
            margin-right: 20px;
        }
    }
}

.content {
    min-height: 450px;
}

.search-card {
    border-radius: 20px;
}
</style>