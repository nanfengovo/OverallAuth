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
</script>

<style lang="less" scoped>
.Menu {}

.menu {
    background-color: #545c64;
    height: 100vh;
}
</style>