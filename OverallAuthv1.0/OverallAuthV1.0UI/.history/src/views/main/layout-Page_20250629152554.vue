<template>
    <div class="common-layout">
        <el-container>
            <el-aside width="200px" class="aside">
                <Menu></Menu>
            </el-aside>
            <el-container>
                <el-header class="header">
                    <Header />
                </el-header>
                <el-main class="main"><router-view></router-view></el-main>
                <el-footer class="footer">
                    <FootPage />
                </el-footer>
            </el-container>
        </el-container>
    </div>
</template>
<script setup lang="ts">
import FootPage from './Foot-Page.vue';
import Menu from './Menu-Page.vue';
import Header from './Header-Page.vue';
</script>

<style lang="less" scoped>
.aside {
    background-color: #545c64;
    height: 100vh;
    width: 200px;
}

.header {
    background-color: #0479ee;
}

.main {
    background-color: #e9eef3;
}

.footer {
    background-color: #e9eef3;
    height: 60px;
}
</style>