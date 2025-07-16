import { Form, Input, Button } from 'antd';

export const AuthForm = ({ form, onSubmit, loading, withConfirmation, tabKey}) => {

  const fieldIdSuffix = `_${tabKey}`;

  return (
    <Form form={form} onFinish={onSubmit}>
      <Form.Item
        name='username'
        rules={[{ required: true, message: 'Enter username' }]}
      >
        <Input placeholder='Username' id={`username${fieldIdSuffix}`} />
      </Form.Item>
      <Form.Item
        name='password'
        rules={[{ required: true, message: 'Enter password' }]}
      >
        <Input.Password placeholder='Password' id={`password${fieldIdSuffix}`} />
      </Form.Item>
      {withConfirmation && (
        <Form.Item
          name='confirmPassword'
          dependencies={['password']}
          rules={[
            { required: true, message: 'Confirm password' },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || getFieldValue('password') === value) {
                  return Promise.resolve();
                }
                return Promise.reject('Passwords do not match!')
              }
            })
          ]}
        >
          <Input.Password placeholder='Confirm password' id={`confirmPassword${fieldIdSuffix}`} />
        </Form.Item>
      )}
      <Form.Item>
        <Button
          type='primary'
          htmlType='submit'
          loading={loading}
          block
        >
          {withConfirmation ? 'Sing Up' : 'Sign In'}
        </Button>
      </Form.Item>
    </Form>
  );
}