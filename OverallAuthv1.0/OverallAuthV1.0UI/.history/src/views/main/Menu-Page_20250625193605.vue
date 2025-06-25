<template>
    <div class="Menu">
        <el-menu default-active="1" class="menu" background-color="#545c64" text-color="#fff"
            active-text-color="#ffd04b">
            <!-- 递归渲染菜单项 -->
            <menu-item v-for="item in menuItems" :key="item.id" :item="item" />

            <!-- 无子菜单 -->
            <el-menu-item v-if="!item.children || item.children.length === 0" :index="item.url">
                <el-icon>
                    <component :is="item.icon" />
                </el-icon>
                <span>{{ item.name }}</span>
            </el-menu-item>

            <!-- 有子菜单 -->
            <el-sub-menu v-else :index="item.url">
                <template #title>
                    <el-icon>
                        <component :is="item.icon" />
                    </el-icon>
                    <span>{{ item.name }}</span>
                </template>
                <menu-item v-for="child in item.children" :key="child.id" :item="child" />
            </el-sub-menu>
        </el-menu>
    </div>
</template>
<script setup lang="ts">


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
        const res = await axios.get('http://localhost:5141/api/Menu/GetAllMenu') // 替换为实际API地址
        console.log('菜单数据:', res.data.data) // 调试输出
        menuItems.value = res.data.data // 根据用户数据结构调整
    } catch (err) {
        console.error('菜单加载失败', err)
    }
}

onMounted(() => {
    fetchMenu()
})

// Removed unused defineProps import and assignment

</script>

<style lang="less" scoped>
.Menu {}

.menu {
    background-color: #545c64;
    height: 100vh;
}
</style>