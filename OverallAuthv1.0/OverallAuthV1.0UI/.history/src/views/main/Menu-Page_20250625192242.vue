<template>
    <div class="Menu">
        <el-menu default-active="2" class="menu" @open="handleOpen" @close="handleClose">
            <el-menu-item index="1">
                <el-icon><icon-menu /></el-icon>
                <span>仪表板</span>
            </el-menu-item>
            <el-menu-item index="2">
                <el-icon><icon-menu /></el-icon>
                <span>用户管理</span>
            </el-menu-item>
            <el-menu-item index="3">
                <el-icon><icon-menu /></el-icon>
                <span>角色管理</span>
            </el-menu-item>
            <el-menu-item index="4">
                <el-icon><icon-menu /></el-icon>
                <span>菜单管理</span>
            </el-menu-item>
        </el-menu>
    </div>
</template>
<script setup lang="ts">
import {
    Menu as IconMenu,
} from '@element-plus/icons-vue'

const handleOpen = (key: string, keyPath: string[]) => {
    console.log(key, keyPath)
}
const handleClose = (key: string, keyPath: string[]) => {
    console.log(key, keyPath)
}

<script setup lang = "ts" >
import { ref, onMounted } from 'vue'
import axios from 'axios'

// 定义菜单项类型
interface MenuItem {
    id: number
    name: string
    icon: string
    url: string
    roles: string[]
    children?: MenuItem[]
}

const menuItems = ref<MenuItem[]>([])

// 从后端获取菜单
const fetchMenu = async () => {
    try {
        const res = await axios.get('/api/menu') // 替换为实际API地址
        menuItems.value = res.data.data // 根据用户数据结构调整
    } catch (err) {
        console.error('菜单加载失败', err)
    }
}

onMounted(() => {
    fetchMenu()
})
</script>

</script>

<style lang="less" scoped>
.Menu {}

.menu {
    background-color: #545c64;
    height: 100vh;
}
</style>