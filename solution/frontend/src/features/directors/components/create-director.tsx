import { Plus } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { useNotifications } from '@/components/ui/notifications'
import { Form, FormDrawer, Input } from '@/components/ui/form'

import { createDirectorInputSchema, useCreateDirector } from '../api/create-director'

export const CreateDirector = () => {
  const { addNotification } = useNotifications()

  const mutation = useCreateDirector({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Director Created'
        })
      }
    }
  })

  return (
    <FormDrawer
      isDone={mutation.isSuccess}
      triggerButton={
        <Button size="sm" icon={<Plus className="size-4" />}>
          Create Director
        </Button>
      }
      title="Create Director"
      submitButton={
        <Button form="create-director" type="submit" size="sm">
          Submit 123
        </Button>
      }
    >
      <Form
        id="create-director"
        onSubmit={(values) => {
          mutation.mutate({ data: values })
        }}
        schema={createDirectorInputSchema}
      >
        {({ register, formState }) => (
          <>
            <Input label="Name" error={formState.errors['name']} registration={register('name')} />
          </>
        )}
      </Form>
    </FormDrawer>
  )
}
