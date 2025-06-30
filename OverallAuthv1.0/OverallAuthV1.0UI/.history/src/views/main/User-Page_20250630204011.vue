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
            <el-button icon="Plus" type="primary" @click="dialogVisible = true">新增用户</el-button>
        </div>
        <div class="content">
            <el-scrollbar max-height="550px">
                <el-table :data=data border style="width: auto;" stripe>

                    <el-table-column align="center" type="selection" width="40px" />
                    <el-table-column align="center" type="index" label="序号" width="60px" />
                    <el-table-column align="center" label="操作" width="130">
                        <!-- <template #default="scope"> -->
                        <el-button type="primary" size="small" text icon="Edit"
                            @click="EditdialogVisible = true">编辑</el-button>
                        <!-- <el-button :type="scope.row.isOpen ? 'danger' : 'primary'" size="small" :text="true"
                                :icon="scope.row.isOpen ? 'CircleCloseFilled' : 'SuccessFilled'">
                                {{ scope.row.isOpen ? '禁用' : '启用' }}
                            </el-button> -->
                        <!-- </template> -->
                    </el-table-column>
                    <el-table-column align="center" prop="name" label="用户名" width="180" />
                    <el-table-column align="center" prop="rolesName" label="角色" width="280" />

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

    <!-- 添加用户对话框 -->
    <el-dialog v-model="dialogVisible" title="添加用户" width="30%" draggable center>
        <el-form :model="Dialogform" label-width="120px">
            <el-form-item label="用户名">
                <el-input v-model="Dialogform.name" />
            </el-form-item>
            <el-form-item label="密码">
                <el-input v-model="Dialogform.password" type="password" show-password />
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
                <el-button type="primary" @click="AddUser">确定</el-button>
            </span>
        </template>
    </el-dialog>

    <!-- 编辑的对话框 -->
    <el-dialog title="编辑用户" width="30%" draggable center v-model="EditdialogVisible">
        <el-form label-width="120px" v-model="EditForm">
            <el-form-item label="用户名">
                <el-input v-model="EditForm.name" />
            </el-form-item>
            <el-form-item label="密码">
                <el-input v-model="EditForm.password" type="password" show-password />
            </el-form-item>
            <el-form-item label="描述">
                <el-input v-model="EditForm.describe" type="textarea" />
            </el-form-item>
            <el-form-item label="是否启用">
                <el-switch v-model="EditForm.isEnable" active-color="#13ce66" inactive-color="#ff4949" />
            </el-form-item>
            <el-form-item label="分配角色">
                <!-- 动态加载已经存在且启用的角色 -->
                <el-select v-model="EditForm.roles" multiple placeholder="请选择角色">
                    <el-option v-for="role in roles" :key="role.id" :label="role.name" :value="role.id">
                    </el-option>
                </el-select>
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="EditdialogVisible = false">取消</el-button>
                <el-button type="primary">确定</el-button>
            </span>
        </template>
    </el-dialog>


</template>


<script setup lang="ts">
// import { ref } from 'vue'
import { ElMessage, type ElForm } from 'element-plus';
import { onMounted } from 'vue';
import { reactive, ref } from 'vue';
import axios from 'axios';

//#region 编辑用户
// 编辑用户对话框
const EditdialogVisible = ref(false);


// 定义编辑用户对话框表单数据
const EditForm = ref({
    name: '',
    password: '',
    describe: '',
    isEnable: false,
    roles: []
});

// 定义角色列表
const roles = ref([]);

//#endregion

//#region 添加用户
//添加用户
const dialogVisible = ref(false);
interface DialogForm {
    name: string;
    password: string;
    describe?: string;
    isEnable: boolean;
}

// 定义添加用户对话框表单数据
const Dialogform = ref<DialogForm>({
    name: '',
    password: '',
    describe: '',
    isEnable: false
});


// 添加用户函数
const AddUser = async () => {
    try {
        //数据合法性验证
        if (!Dialogform.value.name || !Dialogform.value.password) {
            ElMessage.warning('用户名和密码不能为空！');
            return;
        }
        // 发送 POST 请求到后端 API
        const response = await axios.post("http://127.0.0.1:5141/api/User/AddUser", {
            name: Dialogform.value.name,
            pwd: Dialogform.value.password,
            describe: Dialogform.value.describe,
            isEnable: Dialogform.value.isEnable
        });
        // 检查响应状态码
        if (response.data.code === 200) {
            ElMessage.success('添加用户成功！');
            dialogVisible.value = false;
            fetchUserData();
            Dialogform.value = {
                name: '',
                password: '',
                describe: '',
                isEnable: false
            }; // 重置表单数据
        } else {
            ElMessage.error('添加用户失败，请稍后再试！' + response.data.msg);
        }
    }
    catch (error) {
        ElMessage.error('添加用户失败，系统发生异常！' + error);
    }
}

//#endregion

const formRef = ref<InstanceType<typeof ElForm>>()
const searchForm = reactive({
    'name': '',
    'role': ''
})

//重置
function headleResetClick() {
    formRef.value?.resetFields();
}

onMounted(() => {
    fetchUserData();
})

interface User {
    name: string;
    rolesName: string[];
    describe?: string;
    isEnable: boolean;
    createTime?: string;
    updateTime?: string;
}

const data = ref<User[]>([]);

// 获取用户数据
const fetchUserData = async () => {
    try {
        const res = await axios.get("http://127.0.0.1:5141/api/OverallAuth/GetAllUser");
        if (res.data.code === 200) {
            data.value = res.data.data.map((item: User) => ({
                name: item.name,
                rolesName: item.rolesName,
                describe: item.describe || '',
                isEnable: item.isEnable,
                createTime: item.createTime,
                updateTime: item.updateTime
            }));

        } else {
            console.error('获取用户数据失败:', res.data.message);
        }
    } catch (error) {
        console.error('获取用户数据失败:', error);
    }
}




// const data = [{
//     name: '张三',
//     role: '管理员'
// },
// {
//     name: '李四',
//     role: '管理员'
// },
// {
//     name: '张三',
//     role: '管理员'
// },
// {
//     name: '李四',
//     role: '管理员'
// },
// {
//     name: '张三',
//     role: '管理员'
// },
// {
//     name: '李四',
//     role: '管理员'
// },
// {
//     name: '张三',
//     role: '管理员'
// },
// {
//     name: '李四',
//     role: '管理员'
// },
// {
//     name: '张三',
//     role: '管理员'
// }
// ]



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
    min-height: 450px;
}
</style>