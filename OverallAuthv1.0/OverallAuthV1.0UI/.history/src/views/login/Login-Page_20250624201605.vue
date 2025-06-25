<template>
    <div class="login">
        <div v-if="isLoginPage"
            class="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-50 py-12 px-4 sm:px-6 lg:px-8">
            <div class="max-w-md w-full bg-white rounded-xl shadow-lg p-8 space-y-8 relative overflow-hidden">
                <div
                    class="absolute top-0 right-0 w-40 h-40 bg-gradient-to-br from-blue-100 to-indigo-100 rounded-bl-full -z-10">
                </div>
                <div
                    class="absolute bottom-0 left-0 w-40 h-40 bg-gradient-to-tr from-blue-100 to-indigo-100 rounded-tr-full -z-10">
                </div>
                <div>
                    <img src="@/assets/img/Logo.png" width="200" height="200" />
                    <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
                        RBAC 权限管理系统
                    </h2>
                    <p class="mt-2 text-center text-sm text-gray-600">
                        欢迎回来，请登录您的账号
                    </p>
                </div>
                <form class="mt-8 space-y-6" @submit.prevent="handleLogin">
                    <div class="rounded-md shadow-sm space-y-4">
                        <div>
                            <label for="username" class="sr-only">用户名</label>
                            <div class="relative">
                                <el-input id="username" v-model="loginForm.username" type="text" required
                                    placeholder="请输入用户名"
                                    class="appearance-none relative block w-full px-3 py-2 border border-gray-300 rounded-lg"
                                    :prefix-icon="User" />
                            </div>
                        </div>
                        <div>
                            <label for="password" class="sr-only">密码</label>
                            <div class="relative">
                                <el-input id="password" v-model="loginForm.password" type="password" required
                                    placeholder="请输入密码"
                                    class="appearance-none relative block w-full px-3 py-2 border border-gray-300 rounded-lg"
                                    :prefix-icon="Lock" show-password />
                            </div>
                        </div>
                    </div>

                    <div class="flex items-center justify-between">
                        <el-checkbox v-model="loginForm.remember" class="text-sm text-gray-600">
                            记住我
                        </el-checkbox>
                        <div class="text-sm">
                            <a href="#" class="font-medium text-blue-600 hover:text-blue-500">
                                忘记密码？
                            </a>
                        </div>
                    </div>

                    <div>
                        <el-button type="primary" native-type="submit"
                            class="relative w-full !rounded-button whitespace-nowrap cursor-pointer group"
                            :loading="loginLoading">
                            <span class="absolute inset-y-0 left-0 flex items-center pl-3">
                                <el-icon class="h-5 w-5 text-blue-500 group-hover:text-blue-400" v-if="!loginLoading">
                                    <Right />
                                </el-icon>
                            </span>
                            登录系统
                        </el-button>
                    </div>
                </form>

                <div class="mt-6">
                    <p class="text-center text-sm text-gray-600">
                        还没有账号？
                        <a href="#" class="font-medium text-blue-600 hover:text-blue-500">
                            立即注册
                        </a>
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
import { ref } from "vue";
import { ElButton, ElCheckbox, ElInput, ElIcon } from "element-plus";
import { User, Lock, Right } from "@element-plus/icons-vue";

// 登录页状态
const isLoginPage = ref(true);
const loginForm = ref({
    username: "",
    password: "",
    remember: false,
});
const loginLoading = ref(false);



</script>

<style lang="less" scoped>
.login {
    height: 100%;
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #f0f4fe;
    //background: url('../../../src/assets/img/outer-space-photo.jpg');
}
.logo {
    width: 80px;
    height: 80px;
    margin: 0 auto;
    display: block;
}
</style>