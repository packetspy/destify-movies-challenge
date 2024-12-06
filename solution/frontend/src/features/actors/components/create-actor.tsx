import { Plus } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { useNotifications } from '@/components/ui/notifications'
import { Form, FormDrawer, Input } from '@/components/ui/form'

import { createActorInputSchema, useCreateActor } from '../api/create-actor'

export const CreateActor = () => {
  const { addNotification } = useNotifications()

  const createMutation = useCreateActor({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Actor Created'
        })
      }
    }
  })

  return (
    <FormDrawer
      isDone={createMutation.isSuccess}
      triggerButton={
        <Button size="sm" icon={<Plus className="size-4" />}>
          Create Actor
        </Button>
      }
      title="Create Actor"
      submitButton={
        <Button form="create-actor" type="submit" size="sm">
          Submit
        </Button>
      }
    >
      <Form
        id="create-actor"
        onSubmit={(values) => {
          createMutation.mutate({ data: values })
        }}
        schema={createActorInputSchema}
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
