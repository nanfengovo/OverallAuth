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
                <el-button type="primary" icon="Refresh">刷新</el-button>
                <el-button type="danger" icon="delete">删除</el-button>
            </div>
            <el-button icon="Plus" type="primary">新增角色</el-button>
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
</template>
<script setup lang="ts">
// import { ref } from 'vue'

// const data = [{
//     role: '管理员',
//     Permissions: '系统管理,用户管理,角色管理'
// }
// ]

import { ElMessage, type ElForm } from 'element-plus';
import { reactive, ref } from 'vue';
import { onMounted } from 'vue';
import axios from 'axios';

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