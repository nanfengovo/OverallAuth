<template>
    <el-card shadow="hover" class="search-card">
        <el-form label-width="80px" :model="searchForm" ref="formRef">
            <el-row :gutter="20" ref="formRef" justify="center">
                <el-col :span="8">
                    <el-form-item label="菜单名:" prop="name">
                        <el-input type="text" v-model="searchForm.name" placeholder="请输入需要查询的菜单名:" />
                    </el-form-item>
                </el-col>
                <el-col :span="8">
                    <el-form-item label="描述:" prop="describe">
                        <el-input type="text" v-model="searchForm.describe" placeholder="请输入描述的关键字:" />
                    </el-form-item>
                </el-col>
                <el-col :span="8">
                    <el-button type="primary" icon="search" @click="headleSearch">
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
                <el-button type="primary" icon="Refresh" @click="refresh">刷新</el-button>
                <el-button type="danger" icon="delete" @click="deleteMenu">删除</el-button>
            </div>
            <el-button icon="Plus" type="primary" @click="dialogVisible = true">新增菜单</el-button>
        </div>
        <div class="content" v-loading="loading">
            <el-scrollbar max-height="550px">
                <el-table :data=data border style="width: auto;" stripe ref="multipleTableRef">

                    <el-table-column align="center" type="selection" width="40px" />
                    <el-table-column align="center" type="index" label="序号" width="60px" />
                    <el-table-column align="center" label="操作" width="130">
                        <template #default="scope">
                            <el-button type="primary" size="small" text icon="Edit"
                                @click="EditMenu(scope.row)">编辑</el-button>
                            <!-- <el-button :type="scope.row.isOpen ? 'danger' : 'primary'" size="small" :text="true"
                                :icon="scope.row.isOpen ? 'CircleCloseFilled' : 'SuccessFilled'">
                                {{ scope.row.isOpen ? '禁用' : '启用' }}
                            </el-button> -->
                        </template>
                    </el-table-column>
                    <el-table-column align="center" prop="name" label="菜单名" width="180" />
                    <el-table-column align="center" prop="icon" label="图标" width="80">
                        <template #default="scope">
                            <el-icon :size="20">
                                <component :is="scope.row.icon" />
                            </el-icon>
                        </template>
                    </el-table-column>
                    <el-table-column align="center" prop="route" label="路由" width="180" />
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


    <!-- 添加新菜单对话框 -->
    <el-dialog v-model="dialogVisible" title="添加菜单" width="30%" draggable center>
        <el-form :model="Dialogform" label-width="120px">
            <el-form-item label="菜单名">
                <el-input v-model="Dialogform.name" />
            </el-form-item>
            <el-form-item label="图标">
                <div class="icon-selector">
                    <!-- 当前选中的图标预览 -->
                    <div v-if="Dialogform.icon" class="preview">
                        <el-icon :size="24">
                            <component :is="Dialogform.icon" />
                        </el-icon>
                        <span class="icon-name">{{ Dialogform.icon }}</span>
                    </div>

                    <!-- 下拉选择框 -->
                    <el-select v-model="Dialogform.icon" filterable clearable placeholder="请选择图标" class="icon-select"
                        @change="handleIconChange">
                        <el-option v-for="icon in filteredIcons" :key="icon" :value="icon" :label="icon">
                            <div class="icon-option">
                                <el-icon>
                                    <component :is="icon" />
                                </el-icon>
                                <span>{{ icon }}</span>
                            </div>
                        </el-option>
                    </el-select>
                </div>
            </el-form-item>
            <el-form-item label="路由">
                <el-input v-model="Dialogform.route" />
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
                <el-button type="primary" @click="AddMenu">确定</el-button>
            </span>
        </template>
    </el-dialog>


    <!-- 编辑菜单对话框 -->
    <el-dialog v-model="dialogEditVisible" :title="'编辑' + title + '菜单'" width="30%" draggable center>
        <el-form :model="Editform" label-width="120px">
            <el-form-item label="菜单名">
                <el-input v-model="Editform.name" />
            </el-form-item>
            <el-form-item label="图标">
                <div class="icon-selector">
                    <!-- 当前选中的图标预览 -->
                    <div v-if="Dialogform.icon" class="preview">
                        <el-icon :size="24">
                            <component :is="Editform.icon" />
                        </el-icon>
                        <span class="icon-name">{{ Editform.icon }}</span>
                    </div>

                    <!-- 下拉选择框 -->
                    <el-select v-model="Editform.icon" filterable clearable placeholder="请选择图标" class="icon-select"
                        @change="handleIconChange">
                        <el-option v-for="icon in filteredIcons" :key="icon" :value="icon" :label="icon">
                            <div class="icon-option">
                                <el-icon>
                                    <component :is="icon" />
                                </el-icon>
                                <span>{{ icon }}</span>
                            </div>
                        </el-option>
                    </el-select>
                </div>
            </el-form-item>
            <el-form-item label="路由">
                <el-input v-model="Editform.route" />
            </el-form-item>
            <el-form-item label="描述">
                <el-input v-model="Editform.describe" type="textarea" />
            </el-form-item>
            <el-form-item label="是否启用">
                <el-switch v-model="Editform.isEnable" active-color="#13ce66" inactive-color="#ff4949" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="dialogEditVisible = false">取消</el-button>
                <el-button type="primary" @click="Submit(Editform)">确定</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script setup lang="ts">
import type { ElForm, ElTable } from 'element-plus';
import { ElMessage, ElMessageBox } from 'element-plus';
import axios from 'axios';
import { formatTime } from '@/utils/format'
import { ref, computed, onMounted, reactive, nextTick } from 'vue';
import * as ElementPlusIcons from '@element-plus/icons-vue';

//#region 路由开头验证
function isStartsWithSlash(str) {
    const regex = /^\//;
    return regex.test(str);
}
//#endregion

//#region 编辑



const dialogEditVisible = ref(false);
const Editform = ref({
    id: 0, // 用户ID
    name: '',
    icon: '',
    route: '',
    describe: '',
    isEnable: false,
});

const title = ref('');
const EditMenu = async (row: any) => {
    dialogEditVisible.value = true;
    Editform.value = { ...row }
    title.value = row.name;
}

const Submit = async () => {
    const id = Editform.value.id; // 直接获取值
    console.log(Editform.value.name);
    try {
        //数据合法性验证
        if (!Editform.value.name) {
            ElMessage.warning('菜单名不能为空！');
            return;
        }
        else if (isStartsWithSlash(Dialogform.route) === false) {
            ElMessage.error('路由必须以 / 开头');
            return;
        }
        // 发送 POST 请求到后端 API
        const response = await axios.post("http://127.0.0.1:5141/api/Menu/EditMenu?id=" + id, {
            name: Editform.value.name,
            icon: Editform.value.icon,
            url: Editform.value.route,
            describe: Editform.value.describe,
            isEnable: Editform.value.isEnable,
        },
            // {
            //     params: { id } // ✅ 通过 params 传递 id，自动拼接为 ?id=xxx [2,7](@ref)
            // }
        )
        if (response.data.code === 200) {
            ElMessage.success('修改菜单成功');
            dialogEditVisible.value = false;
            fetchMenuData();
        }
        else {
            ElMessage.error('编辑菜单失败:', response.data.code);
        }
    }
    catch (error: any) {
        ElMessage.error('编辑菜单失败:', error);
    }
}  // 检查响应状态码


//#endregion

//#region 菜单数据
//#endregion

//#region 删除
const multipleTableRef = ref<InstanceType<typeof ElTable>>(); // 明确组件类型

const deleteMenu = async () => {
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
            const res = await axios.delete("http://127.0.0.1:5141/api/Menu/DeleteMenus", { data: ids });
            console.log(res.data);
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


    }
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    catch (error) {
        ElMessage.warning('删除失败，用户取消了操作');
    }


}
//#endregion


//#region ---刷新
const isMounted = ref(true);
const loading = ref(false)
const refresh = () => {
    isMounted.value = false;
    setTimeout(() => {
        fetchMenuData();
        loading.value = false; //0.5 后显示内容
    }, 500);
    loading.value = true;
    nextTick(() => {
        isMounted.value = true;
    });
}
//#endregion

//#region 新增菜单
const dialogVisible = ref(false);
// 选中的图标
//const selectedIcon = ref('');
interface DialogForm {
    name: string;
    icon: string;
    route: string;
    describe: string;
    isEnable: boolean;
}

const Dialogform = reactive<DialogForm>({
    name: '',
    icon: '',
    route: '',
    describe: '',
    isEnable: false,
});



const AddMenu = async () => {
    try {
        if (Dialogform.name === '' || Dialogform.route === '' || Dialogform.describe === '') {
            ElMessage.error('请填写完整信息');
            return;
        } else if (isStartsWithSlash(Dialogform.route) === false) {
            ElMessage.error('路由必须以 / 开头');
            return;
        }
        else {
            const res = await axios.post('http://127.0.0.1:5141/api/Menu/AddMenu', {
                name: Dialogform.name,
                icon: Dialogform.icon,
                route: Dialogform.route,
                describe: Dialogform.describe,
                isEnable: Dialogform.isEnable,
            });
            if (res.data.code === 200) {
                ElMessage.success('添加成功');
                dialogVisible.value = false;
                fetchMenuData();
            } else {
                ElMessage.error('添加失败');
            }
        }
    }
    catch (error) {
        ElMessage.error('添加失败' + error);
    }
}


// 获取所有图标名称
const allIcons = Object.keys(ElementPlusIcons);
const icons = ref(allIcons);


const searchQuery = ref('');

// 过滤图标（支持搜索）
const filteredIcons = computed(() => {
    if (!searchQuery.value) return icons.value;
    return icons.value.filter(icon =>
        icon.toLowerCase().includes(searchQuery.value.toLowerCase())
    );
});

// 图标选择处理
const handleIconChange = (value: any) => {
    Dialogform.icon = value;
    emit('update:modelValue', value);
};

// 支持v-model
const props = defineProps(['modelValue']);
const emit = defineEmits(['update:modelValue']);

onMounted(() => {
    if (props.modelValue) {
        Dialogform.icon = props.modelValue;
    }
});

//#endregion

//#region 获取菜单数据
interface Menu {
    id: number;
    name: string;
    icon: string;
    url: string;
    describe: string;
    isEnable: boolean;
    createTime: Date;
    updateTime: string;
}


const data = ref<Menu[]>([]);



onMounted(() => {
    fetchMenuData();
})


const fetchMenuData = async () => {
    try {
        const res = await axios.get('http://127.0.0.1:5141/api/Menu/GetAllMenu');
        if (res.data.code === 200) {
            data.value = res.data.data.map((item: Menu) => ({
                id: item.id,
                name: item.name,
                icon: item.icon,
                route: item.url,
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

// 显式暴露方法
// defineExpose({ fetchMenuData });
//#endregion


//#region 搜索
const headleSearch = async () => {
    try {
        if (searchForm.name === '' && searchForm.describe === '') {
            ElMessage.warning('请输入搜索条件');
            return;
        }
        const name = searchForm.name;
        const describe = searchForm.describe;
        const res = await axios.post("http://127.0.0.1:5141/api/Menu/Search", {
            name: name,
            describe: describe
        });
        if (res.data.code === 200) {
            data.value = res.data.data.map((item: Menu) => ({
                id: item.id,
                name: item.name,
                icon: item.icon,
                route: item.url,
                describe: item.describe || '',
                isEnable: item.isEnable,
                createTime: item.createTime,
                updateTime: item.updateTime || new Date().toISOString()
            }));
        } else {
            ElMessage.error('搜索失败');
        }
    }
    catch (error) {
        ElMessage.error('搜索失败' + error);
    }
}
//#endregion

//#region 重置
const formRef = ref<InstanceType<typeof ElForm>>()
const searchForm = reactive({
    'name': '',
    'describe': '',
})


//重置
function headleResetClick() {
    formRef.value?.resetFields();
    fetchMenuData();
}
//#endregion
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

.icon-selector {
    display: flex;
    align-items: center;
    gap: 12px;
}

.preview {
    display: flex;
    align-items: center;
    gap: 8px;
    min-width: 150px;
    padding: 6px 12px;
    border: 1px solid #dcdfe6;
    border-radius: 4px;
}

.icon-name {
    font-size: 14px;
}

.icon-select {
    flex: 1;
}

.icon-option {
    display: flex;
    align-items: center;
    gap: 8px;
}

.icon-option>span {
    font-size: 13px;
    color: #606266;
}
</style>