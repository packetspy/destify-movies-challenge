import { Plus } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Form, FormDrawer, Input, Textarea } from '@/components/ui/form';
import { useNotifications } from '@/components/ui/notifications';

import {
  createMovieInputSchema,
  useCreateMovie,
} from '../api/create-movie';

export const CreateMovie = () => {
  const { addNotification } = useNotifications();
  const createMovieMutation = useCreateMovie({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Movie Created',
        });
      },
    },
  });

  return (

      <FormDrawer
        isDone={createMovieMutation.isSuccess}
        triggerButton={
          <Button size="sm">
            <Plus className="size-4" />
            Create Movie
          </Button>
        }
        title="Create Movie"
        submitButton={
          <Button
            form="create-movie"
            type="submit"
            size="sm"
          >
            Submit
          </Button>
        }
      >
        <Form
          id="create-movie"
          onSubmit={(values) => {
            createMovieMutation.mutate({ data: values });
          }}
          schema={createMovieInputSchema}
        >
          {({ register, formState }) => (
            <>
              <Input
                label="Title"
                error={formState.errors['title']}
                registration={register('title')}
              />

              <Input
                label="Year"
                error={formState.errors['year']}
                registration={register('year')}
                type='number'
              />

              <Input
                label="Rated"
                error={formState.errors['rated']}
                registration={register('rated')}
              />

              <Input
                label="Genre"
                error={formState.errors['genre']}
                registration={register('genre')}
              />

              <Input
                label="Language"
                error={formState.errors['language']}
                registration={register('language')}
              />

              <Input
                label="Country"
                error={formState.errors['country']}
                registration={register('country')}
              />

              <Input
                label="PPoster"
                error={formState.errors['poster']}
                registration={register('poster')}
              />

              <Textarea
                label="Plot"
                error={formState.errors['plot']}
                registration={register('plot')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
  );
};
