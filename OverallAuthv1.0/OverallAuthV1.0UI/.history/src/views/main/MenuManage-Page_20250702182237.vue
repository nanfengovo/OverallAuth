<template>
    <el-card shadow="hover" class="search-card">
        <el-form label-width="80px" :model="searchForm" ref="formRef">
            <el-row :gutter="20" ref="formRef" justify="center">
                <el-col :span="8">
                    <el-form-item label="菜单名:" prop="name">
                        <el-input type="text" v-model="searchForm.menu" placeholder="请输入需要查询的菜单名:" />
                    </el-form-item>
                </el-col>
                <el-col :span="8">
                    <el-form-item label="描述:" prop="Permissions">
                        <el-input type="text" v-model="searchForm.Permissions" placeholder="请输入包含的权限查询:" />
                    </el-form-item>
                </el-col>
                <el-col :span="8">
                    <el-button type="primary" icon="search">
                        查询
                    </el-button>
                    <el-button icon="Refresh" @click="headleResetClick">
                        重置
                    </el-button>
                </el-col>
            </el-row>
        </el-form>
    </el-card>
    <div class="menu-content">
        <div class="content-top">
            <div class="title">
                <el-button type="primary" icon="Refresh">刷新</el-button>
                <el-button type="danger" icon="delete">删除</el-button>
            </div>
            <el-button icon="Plus" type="primary">新增菜单</el-button>
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
                    <el-table-column align="center" prop="name" label="菜单名" width="180" />
                    <el-table-column align="center" prop="icon" label="图标" width="80">
                        <template #default="scope">
                            <el-icon :size="20">
                                <component :is="scope.row.icon" />
                            </el-icon>
                        </template>
                    </el-table-column>
                    <el-table-column align="center" prop="url" label="路由" width="180" />
                    <el-table-column align="center" prop="describe" label="描述" width="180" />
                    <el-table-column align="center" prop="isEnable" label="是否启用" width="100">
                        <template #default="scope">
                            <el-switch v-model="scope.row.isEnable" disabled active-color="#13ce66"
                                inactive-color="#ff4949" :type="scope.row.isEnable ? 1 : 0" size="small">
                                {{ scope.row.isEnable ? '启用' : '禁用' }}
                            </el-switch>
                        </template>
                    </el-table-column>

                    <el-table-column align="center" prop="createTime" label="创建时间" width="250">
                        <!-- 作用域插槽进行时间格式化(使用Day.js) -->
                        <template #default="scope">
                            {{ formatTime(scope.row.createTime) }}
                        </template>
                    </el-table-column>
                    <el-table-column align="center" prop="updateTime" label="创建时间" width="250">
                        <!-- 作用域插槽进行时间格式化(使用Day.js) -->
                        <template #default="scope">
                            {{ formatTime(scope.row.updateTime) }}
                        </template>
                    </el-table-column>


                </el-table>
            </el-scrollbar>
        </div>


    </div>
</template>
<script setup lang="ts">
import type { ElForm } from 'element-plus';
import { reactive, ref } from 'vue';
import axios from 'axios';
import { onMounted } from 'vue';
import { formatTime } from '@/utils/format'

interface Menu {
    name: string;
    icon: string;
    url: string;
    describe: string;
    isEnable: boolean;
    createTime: Date;
    updateTime: string;
}


const data = ref<Menu[]>([]);

const formRef = ref<InstanceType<typeof ElForm>>()
const searchForm = reactive({
    'Permissions': '',
    'menu': ''
})

onMounted(() => {
    fetchMenuData();
})


const fetchMenuData = async () => {
    try {
        const res = await axios.get('http://127.0.0.1:5141/api/Menu/GetAllMenu');
        if (res.data.code === 200) {
            data.value = res.data.data.map((item: Menu) => ({
                name: item.name,
                icon: item.icon,
                url: item.url,
                describe: item.describe || '',
                isEnable: item.isEnable,
                createTime: item.createTime,
                updateTime: item.updateTime || new Date().toISOString()
            }));
            console.log('菜单数据:', data.value);
        } else {
            console.error('获取菜单数据失败:', res.data.message);
        }
    }
    catch (error) {
        console.error('获取菜单数据失败:', error);
    }
}




//重置
function headleResetClick() {
    formRef.value?.resetFields();
}
</script>

<style lang="less" scoped>
.menu-content {
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