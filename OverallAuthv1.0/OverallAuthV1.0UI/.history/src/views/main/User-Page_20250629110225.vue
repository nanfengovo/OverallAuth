<template>
    <el-card shadow="hover" class="search-card">
        <el-form label-width="80px" :model="searchForm" ref="formRef">
            <el-row :gutter="20" ref="formRef" justify="center">
                <el-col :span="8">
                    <el-form-item label="用户名:" prop="name">
                        <el-input type="text" v-model="searchForm.name" placeholder="请输入需要查询的用户名:" />
                    </el-form-item>
                </el-col>
                <el-col :span="8">
                    <el-form-item label="角色:" prop="role">
                        <el-input type="text" v-model="searchForm.role" placeholder="请输入需要查询的角色:" />
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
    <div class="user-content">
        <div class="content-top">
            <div class="title">
                <el-button type="primary" icon="Refresh">刷新</el-button>
                <el-button type="danger" icon="delete">删除</el-button>
            </div>
            <el-button icon="Plus" type="primary">新增用户</el-button>
        </div>
        <div class="content">
            <el-scrollbar max-height="550px">
                <el-table :data=data border style="width: auto;" stripe>
                    <el-table-column align="center" type="selection" width="40px" />
                    <el-table-column align="center" type="index" label="序号" width="60px" />
                    <el-table-column align="center" prop="name" label="用户名" width="180" />
                    <el-table-column align="center" prop="role" label="角色" width="380" />
                    <el-table-column align="center" prop="describe" label="描述" width="180" />
                    <el-switch v-model="value2" class="ml-2"
                        style="--el-switch-on-color: #13ce66; --el-switch-off-color: #ff4949" />
                    <el-table-column align="center" label="操作" width="230">
                        <template #default="scope">
                            <el-button type="primary" size="small" text icon="Edit">编辑</el-button>
                            <el-button :type="scope.row.isOpen ? 'danger' : 'primary'" size="small" :text="true"
                                :icon="scope.row.isOpen ? 'CircleCloseFilled' : 'SuccessFilled'">
                                {{ scope.row.isOpen ? '禁用' : '启用' }}
                            </el-button>
                        </template>
                    </el-table-column>
                </el-table>
            </el-scrollbar>
        </div>


    </div>
</template>
<script setup lang="ts">
// import { ref } from 'vue'
import type { ElForm } from 'element-plus';
import { reactive, ref } from 'vue';

const formRef = ref<InstanceType<typeof ElForm>>()
const searchForm = reactive({
    'name': '',
    'role': ''
})

//重置
function headleResetClick() {
    formRef.value?.resetFields();
}

const data = [{
    name: '张三',
    role: '管理员'
},
{
    name: '李四',
    role: '管理员'
},
{
    name: '张三',
    role: '管理员'
},
{
    name: '李四',
    role: '管理员'
},
{
    name: '张三',
    role: '管理员'
},
{
    name: '李四',
    role: '管理员'
},
{
    name: '张三',
    role: '管理员'
},
{
    name: '李四',
    role: '管理员'
},
{
    name: '张三',
    role: '管理员'
}
]
</script>

<style lang="less" scoped>
.search-card {
    border-radius: 20px;
}

.user-content {
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
    min-height: 550px;
}
</style>