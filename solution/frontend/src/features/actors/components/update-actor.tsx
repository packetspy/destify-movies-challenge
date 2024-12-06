import { Pen } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Form, FormDrawer, Input } from '@/components/ui/form'
import { useNotifications } from '@/components/ui/notifications'

import { useActor } from '../api/get-actor'
import { updateActorInputSchema, useUpdateActor } from '../api/update-actor'

type UpdateActorProps = {
  uniqueId: string
}

export const UpdateActor = ({ uniqueId }: UpdateActorProps) => {
  const { addNotification } = useNotifications()
  const query = useActor({ uniqueId })

  const mutation = useUpdateActor({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Actor Updated',
          message: 'The actor was successfull'
        })

        query.refetch()
      }
    }
  })
  const actor = query.data?.data

  return (
    <FormDrawer
      isDone={mutation.isSuccess}
      triggerButton={
        <Button size="sm" icon={<Pen className="size-4" />}>
          Update Actor
        </Button>
      }
      title="Update Actor"
      submitButton={
        <>
          <Button form="update-actor" type="submit" size="sm" isLoading={mutation.isPending}>
            Submit
          </Button>
        </>
      }
    >
      <Form
        id="update-actor"
        onSubmit={(values) => {
          values.movies = actor?.movies ?? []
          mutation.mutate({
            data: values,
            uniqueId
          })
        }}
        options={{
          defaultValues: {
            uniqueId: actor?.uniqueId ?? '',
            movies: actor?.movies ?? [],
            name: actor?.name ?? ''
          }
        }}
        schema={updateActorInputSchema}
      >
        {({ register, formState }) => {
          return (
            <>
              <Input
                label="Name"
                error={formState.errors['name']}
                registration={register('name')}
              />
            </>
          )
        }}
      </Form>
    </FormDrawer>
  )
}
