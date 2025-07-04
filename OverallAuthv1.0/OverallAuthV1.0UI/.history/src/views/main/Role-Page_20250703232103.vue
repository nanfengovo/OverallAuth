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
    <div class="role-content" v-loading="loading">
        <div class="content-top">
            <div class="title">
                <el-button type="primary" icon="Refresh" @click="refresh">刷新</el-button>
                <el-button type="danger" icon="delete">删除</el-button>
            </div>
            <el-button icon="Plus" type="primary" @click="headleAddClick">新增角色</el-button>
        </div>
        <div class="content">
            <el-scrollbar max-height="550px">
                <el-table :data=data border style="width: auto;" stripe>

                    <el-table-column align="center" type="selection" width="40px" />
                    <el-table-column align="center" type="index" label="序号" width="60px" />
                    <el-table-column align="center" label="操作" width="130">
                        <!-- <template #default="scope"> -->
                        <el-button type="primary" size="small" text icon="Edit">编辑</el-button>
                        <!-- <el-button :type="scope.row.isOpen ? 'danger' : 'primary'" size="small" :text="true"
                                :icon="scope.row.isOpen ? 'CircleCloseFilled' : 'SuccessFilled'">
                                {{ scope.row.isOpen ? '禁用' : '启用' }}
                            </el-button> -->
                        <!-- </template> -->
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
            <el-form-item label="菜单">
                <el-tree style="max-width: 600px" :props="props" :load="loadNode" lazy show-checkbox
                    @check-change="handleCheckChange" />
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

</template>
<script setup lang="ts">
// import { ref } from 'vue'

// const data = [{
//     role: '管理员',
//     Permissions: '系统管理,用户管理,角色管理'
// }
// ]

import { ElMessage, type ElForm } from 'element-plus';
import type { LoadFunction } from 'element-plus'
import { reactive, ref, nextTick } from 'vue';
import { onMounted } from 'vue';
import axios from 'axios';

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


interface Tree {
    name: string
}
let count = 1
const props = {
    label: 'name',
    children: 'zones',
}

const handleCheckChange = (
    data: Tree,
    checked: boolean,
    indeterminate: boolean
) => {
    console.log(data, checked, indeterminate)
}

const loadNode: LoadFunction = (node, resolve) => {
    if (node.level === 0) {
        return resolve([{ name: 'Home' }, { name: 'User' }, { name: 'Role' }, { name: 'menu' }])
    }
    if (node.level > 3) return resolve([])

    let hasChild = false
    if (node.data.name === 'Home') {
        hasChild = false
    } else if (node.data.name === 'User') {
        hasChild = false
    } else if (node.data.name === 'Role') {
        hasChild = false
    } else if (node.data.name === 'menu') {
        hasChild = false
    }
    else {
        hasChild = Math.random() > 0.5
    }
    setTimeout(() => {
        let data: Tree[] = []
        if (hasChild) {
            data = [
                {
                    name: `zone${count++}`,
                },
                {
                    name: `zone${count++}`,
                },
            ]
        } else {
            data = []
        }

        resolve(data)
    }, 500)
}

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


onMounted(() => {
    fetchRoleData();
})

interface Role {
    name: string;
    menusName: string[];
    describe?: string;
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
const dialogVisible = ref(false);

interface DialogForm {
    name: string;
    password: string;
    describe: string;
    isEnable: boolean;
}

const Dialogform = reactive<DialogForm>({
    name: '',
    password: '',
    describe: '',
    isEnable: true
})

const AddRole = async () => {
    try {

    }
    catch (error) {

    }
}

const headleAddClick = async () => {
    dialogVisible.value = true;
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