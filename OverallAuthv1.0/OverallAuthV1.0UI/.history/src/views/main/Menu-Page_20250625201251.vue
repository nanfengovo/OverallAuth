<template>
    <div class="logo">
        <img src="../../assets/img/logo.jpg" alt="">
    </div>
    <div class="menu-container">
        <el-menu :default-active="activeMenu" class="el-menu-vertical" background-color="#545c64" text-color="#fff"
            active-text-color="#ffd04b" router>
            <template v-for="item in filteredMenu" :key="item.id">
                <menu-item :item="item" />
            </template>
        </el-menu>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
import { useRoute } from 'vue-router'
import MenuItem from './MenuItem.vue'

// 定义菜单项类型
interface MenuItem {
    id: number
    name: string
    icon: string
    url: string
    roles: string[]
    children?: MenuItem[]
    isEnable: boolean
}

const route = useRoute()
const menuItems = ref<MenuItem[]>([])

// 获取当前激活菜单项（根据路由路径）
const activeMenu = computed(() => route.path)

// 过滤可用且有权限的菜单
const filteredMenu = computed(() => {
    const userRoles = ['admin'] // 从Vuex/Pinia获取实际用户角色
    return menuItems.value.filter(item =>
        item.isEnable &&
        (item.roles.length === 0 || item.roles.some(r => userRoles.includes(r)))
    )
})

// 从后端获取菜单
const fetchMenu = async () => {
    try {
        const res = await axios.get('http://localhost:5141/api/Menu/GetAllMenu')
        console.log('菜单数据:', res.data.data)
        menuItems.value = res.data.data
    } catch (err) {
        console.error('菜单加载失败', err)
    }
}

onMounted(() => {
    fetchMenu()
})
</script>

<style lang="less" scoped>
.logo {
    height: 60px;
    width: 280px;
}

.logo img {
    width: 100%;
    height: 100%;
    object-fit: contain;
    /* 保持比例，可能留白 */
}

.menu-container {
    height: 100vh;
    overflow-y: hidden;
    background-color: #545c64;
    overflow-x: hidden;

    .el-menu-vertical {
        border-right: none;
        height: 100%;

        &:not(.el-menu--collapse) {
            width: 220px;
        }
    }
}
</style>