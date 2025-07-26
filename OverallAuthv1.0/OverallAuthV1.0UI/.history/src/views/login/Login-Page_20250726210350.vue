<template>
    <div class="login-container">
        <!-- 背景装饰元素 -->
        <div class="background-decoration">
            <div class="decoration-circle decoration-circle-1"></div>
            <div class="decoration-circle decoration-circle-2"></div>
            <div class="floating-shapes">
                <div class="shape shape-1"></div>
                <div class="shape shape-2"></div>
                <div class="shape shape-3"></div>
            </div>
        </div>

        <div class="login-card">
            <!-- Logo和标题区域 -->
            <div class="header-section">
                <div class="logo-container">
                    <div class="logo-icon">
                        <el-icon size="32" color="#ffffff">
                            <Shield />
                        </el-icon>
                    </div>
                </div>
                <h1 class="title">RBAC 权限管理系统</h1>
                <p class="subtitle">欢迎回来，请登录您的账号</p>
            </div>

            <!-- 错误提示 -->
            <transition name="slide-down">
                <el-alert v-if="errorMessage" :title="errorMessage" type="error" :closable="false"
                    class="error-alert" />
            </transition>

            <!-- 登录表单 -->
            <el-form ref="loginFormRef" :model="loginForm" :rules="loginRules" class="login-form"
                @submit.prevent="handleLogin">
                <el-form-item prop="username" class="form-item">
                    <div class="input-wrapper">
                        <el-input v-model="loginForm.username" placeholder="请输入用户名" size="large" class="custom-input"
                            :prefix-icon="User" @input="clearError" />
                    </div>
                </el-form-item>

                <el-form-item prop="password" class="form-item">
                    <div class="input-wrapper">
                        <el-input v-model="loginForm.password" type="password" placeholder="请输入密码" size="large"
                            class="custom-input" :prefix-icon="Lock" show-password @input="clearError" />
                    </div>
                </el-form-item>

                <div class="form-options">
                    <el-checkbox v-model="loginForm.remember" class="remember-checkbox">
                        记住我
                    </el-checkbox>
                    <el-link type="primary" class="forgot-password">
                        忘记密码？
                    </el-link>
                </div>

                <el-button type="primary" size="large" class="login-button" :loading="loginLoading"
                    @click="handleLogin">
                    <template #loading>
                        <div class="loading-content">
                            <span>登录中...</span>
                        </div>
                    </template>
                    <div class="button-content">
                        <span>登录系统</span>
                        <el-icon class="arrow-icon">
                            <ArrowRight />
                        </el-icon>
                    </div>
                </el-button>
            </el-form>

            <!-- 注册链接 -->
            <div class="register-section">
                <span class="register-text">还没有账号？</span>
                <el-link type="primary" class="register-link">立即注册</el-link>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import {
    ElButton,
    ElCheckbox,
    ElInput,
    ElIcon,
    ElMessage,
    ElForm,
    ElFormItem,
    ElAlert,
    ElLink,
    type FormInstance,
    type FormRules
} from "element-plus";
import { User, Lock, ArrowRight } from "@element-plus/icons-vue";
import axios from 'axios';
import { useRouter } from 'vue-router';

// 响应式数据
const loginFormRef = ref<FormInstance>();
const loginForm = reactive({
    username: "",
    password: "",
    remember: false,
});

const loginLoading = ref(false);
const errorMessage = ref("");
const router = useRouter();

// 表单验证规则
const loginRules: FormRules = {
    username: [
        { required: true, message: '请输入用户名', trigger: 'blur' },
        { min: 2, max: 20, message: '用户名长度在 2 到 20 个字符', trigger: 'blur' }
    ],
    password: [
        { required: true, message: '请输入密码', trigger: 'blur' },
        { min: 6, message: '密码长度不能少于 6 个字符', trigger: 'blur' }
    ]
};

// 清除错误信息
const clearError = () => {
    if (errorMessage.value) {
        errorMessage.value = "";
    }
};

// 登录处理函数
const handleLogin = async () => {
    if (!loginFormRef.value) return;

    try {
        // 表单验证
        await loginFormRef.value.validate();

        loginLoading.value = true;
        errorMessage.value = "";

        console.log(loginForm.username, loginForm.password);

        const res = await axios.get('http://127.0.0.1:5141/api/OverallAuth/Login', {
            params: {
                userName: loginForm.username,
                password: loginForm.password
            },
            timeout: 10000
        });

        if (res.data.code === 200) {
            // 存储token和用户信息
            localStorage.setItem('token', "TOKEN");
            localStorage.setItem('username', loginForm.username);

            // 显示成功消息
            ElMessage.success('登录成功！');
            ElMessage.success(`欢迎 ${loginForm.username} 使用 RBAC 权限管理系统！`);

            // 跳转主页
            router.push('/Home');
        } else {
            errorMessage.value = res.data.message || "登录失败，请检查用户名和密码";
        }
    } catch (error: any) {
        console.error("Login error:", error);
        if (error.response) {
            errorMessage.value = error.response.data?.message || "服务器错误，请稍后重试";
        } else if (error.request) {
            errorMessage.value = "网络连接失败，请检查网络设置";
        } else {
            errorMessage.value = "登录失败，请稍后重试";
        }
    } finally {
        loginLoading.value = false;
    }
};
</script>

<style lang="scss" scoped>
.login-container {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    padding: 20px;
    position: relative;
    overflow: hidden;
}

.background-decoration {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    pointer-events: none;
    z-index: 0;
}

.decoration-circle {
    position: absolute;
    border-radius: 50%;
    background: rgba(255, 255, 255, 0.1);
    backdrop-filter: blur(10px);

    &-1 {
        width: 300px;
        height: 300px;
        top: -150px;
        right: -150px;
        animation: float 6s ease-in-out infinite;
    }

    &-2 {
        width: 200px;
        height: 200px;
        bottom: -100px;
        left: -100px;
        animation: float 8s ease-in-out infinite reverse;
    }
}

.floating-shapes {
    .shape {
        position: absolute;
        background: rgba(255, 255, 255, 0.05);
        border-radius: 10px;

        &-1 {
            width: 60px;
            height: 60px;
            top: 20%;
            left: 10%;
            animation: rotate 10s linear infinite;
        }

        &-2 {
            width: 40px;
            height: 40px;
            top: 60%;
            right: 15%;
            animation: rotate 15s linear infinite reverse;
        }

        &-3 {
            width: 80px;
            height: 80px;
            bottom: 20%;
            left: 20%;
            animation: rotate 12s linear infinite;
        }
    }
}

.login-card {
    width: 100%;
    max-width: 420px;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(20px);
    border-radius: 24px;
    padding: 40px;
    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
    position: relative;
    z-index: 1;
    border: 1px solid rgba(255, 255, 255, 0.2);
}

.header-section {
    text-align: center;
    margin-bottom: 32px;
}

.logo-container {
    display: flex;
    justify-content: center;
    margin-bottom: 24px;
}

.logo-icon {
    width: 64px;
    height: 64px;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    border-radius: 16px;
    display: flex;
    align-items: center;
    justify-content: center;
    box-shadow: 0 8px 24px rgba(102, 126, 234, 0.3);
}

.title {
    font-size: 28px;
    font-weight: 700;
    color: #1a202c;
    margin: 0 0 8px 0;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    background-clip: text;
}

.subtitle {
    color: #718096;
    font-size: 16px;
    margin: 0;
}

.error-alert {
    margin-bottom: 24px;
    border-radius: 12px;

    :deep(.el-alert__content) {
        font-size: 14px;
    }
}

.login-form {
    .form-item {
        margin-bottom: 20px;

        :deep(.el-form-item__error) {
            font-size: 12px;
            margin-top: 4px;
        }
    }
}

.input-wrapper {
    :deep(.custom-input) {
        .el-input__wrapper {
            border-radius: 12px;
            border: 2px solid #e2e8f0;
            box-shadow: none;
            transition: all 0.3s ease;
            background: #f8fafc;

            &:hover {
                border-color: #cbd5e0;
            }

            &.is-focus {
                border-color: #667eea;
                background: #ffffff;
                box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
            }
        }

        .el-input__inner {
            font-size: 16px;
            color: #2d3748;

            &::placeholder {
                color: #a0aec0;
            }
        }
    }
}

.form-options {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 24px;
}

.remember-checkbox {
    :deep(.el-checkbox__label) {
        color: #4a5568;
        font-size: 14px;
    }
}

.forgot-password {
    font-size: 14px;
    text-decoration: none;

    &:hover {
        text-decoration: underline;
    }
}

.login-button {
    width: 100%;
    height: 48px;
    border-radius: 12px;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    border: none;
    font-size: 16px;
    font-weight: 600;
    margin-bottom: 24px;
    transition: all 0.3s ease;

    &:hover {
        transform: translateY(-2px);
        box-shadow: 0 8px 24px rgba(102, 126, 234, 0.4);
    }

    &:active {
        transform: translateY(0);
    }

    .button-content {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
    }

    .arrow-icon {
        transition: transform 0.3s ease;
    }

    &:hover .arrow-icon {
        transform: translateX(4px);
    }

    .loading-content {
        display: flex;
        align-items: center;
        gap: 8px;
    }
}

.register-section {
    text-align: center;
    padding-top: 20px;
    border-top: 1px solid #e2e8f0;
}

.register-text {
    color: #718096;
    font-size: 14px;
    margin-right: 8px;
}

.register-link {
    font-size: 14px;
    font-weight: 600;
    text-decoration: none;

    &:hover {
        text-decoration: underline;
    }
}

// 动画定义
@keyframes float {

    0%,
    100% {
        transform: translateY(0px);
    }

    50% {
        transform: translateY(-20px);
    }
}

@keyframes rotate {
    from {
        transform: rotate(0deg);
    }

    to {
        transform: rotate(360deg);
    }
}

// 过渡动画
.slide-down-enter-active,
.slide-down-leave-active {
    transition: all 0.3s ease;
}

.slide-down-enter-from {
    opacity: 0;
    transform: translateY(-10px);
}

.slide-down-leave-to {
    opacity: 0;
    transform: translateY(-10px);
}

// 响应式设计
@media (max-width: 480px) {
    .login-card {
        margin: 20px;
        padding: 32px 24px;
    }

    .title {
        font-size: 24px;
    }

    .subtitle {
        font-size: 14px;
    }
}
</style>