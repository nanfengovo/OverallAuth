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
                        <el-input type="text" v-model="searchForm.Permissions" placeholder="请输入描述的关键字:" />
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
            <el-button icon="Plus" type="primary" @click="dialogVisible = true">新增菜单</el-button>
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


    <!-- 添加新菜单对话框 -->
    <el-dialog v-model="dialogVisible" title="添加菜单" width="30%" draggable center>
        <el-form :model="Dialogform" label-width="120px">
            <el-form-item label="菜单名">
                <el-input v-model="Dialogform.name" />
            </el-form-item>
            <el-form-item label="图标">
                <div class="icon-selector">
                    <!-- 当前选中的图标预览 -->
                    <div v-if="selectedIcon" class="preview">
                        <el-icon :size="24">
                            <component :is="selectedIcon" />
                        </el-icon>
                        <span class="icon-name">{{ selectedIcon }}</span>
                    </div>

                    <!-- 下拉选择框 -->
                    <el-select v-model="selectedIcon" filterable clearable placeholder="请选择图标" class="icon-select"
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
</template>
<script setup lang="ts">
import type { ElForm } from 'element-plus';
import { ElMessage } from 'element-plus';
import axios from 'axios';
import { formatTime } from '@/utils/format'
import { ref, computed, onMounted, reactive } from 'vue';
import * as ElementPlusIcons from '@element-plus/icons-vue';

//#region 新增菜单
const dialogVisible = ref(false);
// 选中的图标
const selectedIcon = ref('');
interface DialogForm {
    name: string;
    //selectedIcon: string;
    route: string;
    describe: string;
    isEnable: boolean;
}

const Dialogform = reactive<DialogForm>({
    name: '',
    //selectedIcon: '',
    route: '',
    describe: '',
    isEnable: false,
});

const AddMenu = async () => {
    try {
        if (Dialogform.name === '' || Dialogform.route === '' || Dialogform.describe === '') {
            ElMessage.error('请填写完整信息');
            return;
        }
        else {
            const res = await axios.post('http://127.0.0.1:5141/api/Menu/AddMenu', {
                name: Dialogform.name,
                //icon: selectedIcon,
                url: Dialogform.route,
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
    selectedIcon.value = value;
    emit('update:modelValue', value);
};

// 支持v-model
const props = defineProps(['modelValue']);
const emit = defineEmits(['update:modelValue']);

onMounted(() => {
    if (props.modelValue) {
        selectedIcon.value = props.modelValue;
    }
});

//#endregion

//#region 获取菜单数据
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

//#endregion


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